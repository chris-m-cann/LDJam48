using UnityEditor;
using UnityEngine;

namespace Util
{
    // nasty workaround tool as unity doesnt let you edit nested asserts the normal way
    public class NestedAssetWindow : EditorWindow
    {
        [MenuItem("Tools/ChangeNestedAsset")]
        [MenuItem("Assets/ChangeNestedAsset")]
        private static void ShowWindow()
        {
            var window = GetWindow<NestedAssetWindow>();
            window.titleContent = new GUIContent("Change Nested Asset Window");
            window.Show();
        }

        private SerializedObject _so;

        private SerializedProperty _propTarget;
        private SerializedProperty _propNewName;

        public Object Target;
        public string NewName;

        private void OnEnable()
        {
            _so = new SerializedObject(this);
            _propTarget = _so.FindProperty(nameof(Target));
            _propNewName = _so.FindProperty(nameof(NewName));
        }

        private void OnGUI()
        {
            using (_so.UpdateScope())
            {
                EditorGUILayout.PropertyField(_propTarget);
                EditorGUILayout.PropertyField(_propNewName);
                
                if (GUILayout.Button("Rename"))
                {
                    _propTarget.objectReferenceValue.name = _propNewName.stringValue;
                    
                    AssetDatabase.SaveAssets();
                }

                if (GUILayout.Button("Delete"))
                {
                    AssetDatabase.RemoveObjectFromAsset(_propTarget.objectReferenceValue);
                    AssetDatabase.SaveAssets();
                }
            }
            
            this.RemoveFocusOnMouseDown();
        }
    }
}