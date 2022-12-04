using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Util.Scenes
{
    // open scenes on asset double click
    public class SceneReferenceAssetHandler
    {
        [OnOpenAsset]
        public static bool OpenScenes(int instanceId, int line)
        {
            if (EditorUtility.InstanceIDToObject(instanceId) is SceneReference asset && asset != null)
            {

                SceneReferenceEditor.OpenScenes(asset, true);
                return true;
            }

            return false;
        }
    }
    
    [CustomEditor(typeof(SceneReference))]
    public class SceneReferenceEditor : Editor
    {
        private const string NEW_ASSET_PREFIX = "";
        private const string NEW_ASSET_SUFFIX = "_SRef";
        public override void OnInspectorGUI()
        {
            var sref = (SceneReference)target;
            if (sref == null) return;
            
            
            base.OnInspectorGUI();

            if (GUILayout.Button("Open"))
            {
                OpenScenes(sref, true);
            }
            
            if (GUILayout.Button("Open (Additive)"))
            {
                OpenScenes(sref, false);
            }
        }

        public static void OpenScenes(SceneReference sref, bool unloadOthers)
        {
            // create a set of all required scenes
            var dependants = new HashSet<string>();
            dependants = GetAllDependants(sref, dependants);

            // find all open scenes that arnt in set
            List<Scene> unneededScenes = new();

            for (int i = 0; i < EditorSceneManager.loadedSceneCount; i++)
            {
                var scene = EditorSceneManager.GetSceneAt(i);

                if (!dependants.Contains(scene.path))
                {
                    unneededScenes.Add(scene);
                }
            }

            bool unloadingAll = unloadOthers && unneededScenes.Count == EditorSceneManager.loadedSceneCount;

            if (unloadOthers)
            {
                // offer to save those scenes
                EditorSceneManager.SaveModifiedScenesIfUserWantsTo(unneededScenes.ToArray());
                foreach (var scene in unneededScenes)
                {
                    EditorSceneManager.CloseScene(scene, true);
                }
            }

            // open all scenes in set that arnt already open
            for (int i = 0; i < EditorSceneManager.loadedSceneCount; i++)
            {
                var scene = EditorSceneManager.GetSceneAt(i);

                dependants.Remove(scene.path);
            }

            OpenSceneMode mode = unloadingAll ? OpenSceneMode.Single : OpenSceneMode.Additive;
            foreach (var dependant in dependants)
            {
                EditorSceneManager.OpenScene(dependant, mode);
                mode = OpenSceneMode.Additive;
            }
        }

        public static HashSet<string> GetAllDependants(SceneReference sref, HashSet<string> dependants) {
            dependants.Add(sref.scene);
            foreach (var required in sref.requires)
            {
                if (dependants.Contains(required.scene)) continue;

                dependants = GetAllDependants(required, dependants);
            }

            return dependants;
        }
        
        
        [MenuItem("Assets/Create/Custom/SceneReference")]
        public static void CreateAssets()
        {
            List<Object> selection = new();
            foreach (Object obj in Selection.GetFiltered(typeof(Object), SelectionMode.Assets))
            {
                var path = AssetDatabase.GetAssetPath(obj);
                if (string.IsNullOrEmpty(path)) continue;
                if (Directory.Exists(path))
                {
                    selection.Add(CreateDefault(path));
                } 
                if (!string.IsNullOrEmpty(path) && File.Exists(path) && obj is SceneAsset asset)
                {
                    selection.Add(CreateFromScene(path, asset));
                }
            }
            AssetDatabase.SaveAssets();
            
            EditorUtility.FocusProjectWindow();

            Selection.objects = selection.ToArray();
        }

        private static Object CreateFromScene(string path, SceneAsset asset)
        {
            var dir = Path.GetDirectoryName(path);
            SceneReference v = ScriptableObject.CreateInstance<SceneReference>();
            v.scene = path;
            
            var name = NEW_ASSET_PREFIX + asset.name + NEW_ASSET_SUFFIX + ".asset";
            AssetDatabase.CreateAsset(v, dir + "/"+ name);
            return v;
        }

        private static Object CreateDefault(string path)
        {
            SceneReference v = ScriptableObject.CreateInstance<SceneReference>();
            
            string name = $"New {nameof(SceneReference)}.asset";
            AssetDatabase.CreateAsset(v, path + "/"+ name);
            return v;
        }
    }
}