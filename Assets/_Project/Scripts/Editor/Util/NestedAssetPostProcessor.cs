using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Util
{
    public class NestedAssetPostProcessor : AssetPostprocessor
    {
        static void OnPostprocessAllAssets(
            string[] importedAssets, 
            string[] deletedAssets, 
            string[] movedAssets,
            string[] movedFromAssetPaths
            )
        {
            foreach (string str in importedAssets)
            {
                var asset = AssetDatabase.LoadAssetAtPath<ScriptableObject>(str);
                if (asset == null) continue;

                var movedAssetIdx = Array.IndexOf(movedAssets, str);
                
                var created = movedAssetIdx == -1;
                if (created)
                {
                    AddNestedAssets(str, asset);
                }
                else
                {
                    RenameNestedAssets(str, movedFromAssetPaths[movedAssetIdx], asset);
                }

            }
        }

        private static void RenameNestedAssets(string str, string movedFromAssetPath, ScriptableObject asset)
        {
            var lastSlash = movedFromAssetPath.LastIndexOf('/');
            var dot = movedFromAssetPath.LastIndexOf('.');
            var oldName = movedFromAssetPath.Substring(lastSlash + 1, dot - (lastSlash + 1));

            var found = AssetDatabase.LoadAllAssetRepresentationsAtPath(str);

            foreach (var subasset in found)
            {
                subasset.name = subasset.name.Replace(oldName, asset.name);
            }

            AssetDatabase.SaveAssets();
        }

        public static void AddNestedAssets(string path, Object obj)
        {
            if (AddNestedAssets(path, obj, new List<Type>(), obj.name + "-"))
            {
                AssetDatabase.SaveAssets();
            }
        }
        private static bool AddNestedAssets(string path, Object obj, List<Type> typeChain, string namePrefix)
        {
            var reimport = false;

            foreach (var field in obj.GetType()
                .GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
            {
                // only operate on [Nested] members
                if (Attribute.GetCustomAttribute(field, typeof(NestedAttribute)) == null)
                {
                    continue;
                }
                
                // nested SO already been created and linked
                if (field.GetValue(obj) != null) continue;
                // nested SO already been created but just manually unlinked by user
                if (AssetDatabase.FindAssets(BuildName(namePrefix, field.Name), new []{path}).Length > 0) continue;
                
                if (!field.FieldType.HasBase<ScriptableObject>())
                {
                    Debug.LogWarning($"{obj.GetType().Name}.{field.Name}: Nested attribute only valid for fields that derive from ScriptableObject");
                    continue;
                }

                if (field.FieldType.IsAbstract)
                {
                    Debug.LogWarning($"{obj.GetType().Name}{field.Name}: Nested attribute only valid for fields with concrete types");
                    continue;
                }
                
                // try to solve infinite recursion
                if (typeChain.Contains(field.FieldType))
                {
                    var chainString = String.Join(", ", typeChain.Select(it => it.Name));
                    Debug.LogWarning($"{obj.GetType().Name}.{field.Name}: Nested attribute infinite recursion detected, loop = {chainString}");
                    continue;
                }
                
                
                CreateNestedMember(obj, field, typeChain, namePrefix);

                reimport = true;
            }

            return reimport;
        }

        private static void CreateNestedMember(Object obj, FieldInfo field, List<Type> typeChain, string namePrefix)
        {
            var asset = ScriptableObject.CreateInstance(field.FieldType);
            field.SetValue(obj, asset);
            var nameProp = field.FieldType.GetProperties().FirstOrDefault(it => it.Name == "name");
            if (nameProp != null)
            {
                nameProp.SetValue(field.GetValue(obj), BuildName(namePrefix, field.Name));
            }

            AssetDatabase.AddObjectToAsset(asset, obj);
            
            
            typeChain.Add(field.FieldType);
            AddNestedAssets(AssetDatabase.GetAssetPath(asset), asset, typeChain, BuildName(namePrefix, field.Name) + "-");
        }

        private static string BuildName(string prefix, string fieldName)
        {
            return prefix + fieldName;
        }
    }
}