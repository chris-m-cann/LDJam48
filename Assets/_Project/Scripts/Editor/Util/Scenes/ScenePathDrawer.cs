using UnityEditor;
using UnityEngine;

namespace Util.Scenes
{
    [CustomPropertyDrawer(typeof(ScenePathAttribute))]
    public class ScenePathDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            property.serializedObject.Update();
            var oldPath = property.stringValue;
            
            var oldScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(oldPath);

            using var change = new EditorGUI.ChangeCheckScope();
            
            var newScene = EditorGUILayout.ObjectField(label, oldScene, typeof(SceneAsset), false) as SceneAsset;

            if (change.changed)
            {
                var newPath = AssetDatabase.GetAssetPath(newScene);
                property.stringValue = newPath;
            }

            property.serializedObject.ApplyModifiedProperties();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight;
        }
    }
}