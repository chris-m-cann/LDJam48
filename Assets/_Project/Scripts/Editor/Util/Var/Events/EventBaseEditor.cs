using UnityEditor;
using UnityEngine;

namespace Util.Var.Events
{
    [CustomEditor(typeof(GameEvent<>), editorForChildClasses:true)]
    public class EventBaseEditor : Editor
    {
        public override void OnInspectorGUI()
        {

            using (new EditorEx.ModifyObjectScope(serializedObject))
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("Value"));
                var prev = GUI.enabled;
                try
                {
                    GUI.enabled = EditorApplication.isPlaying;
                    if (GUILayout.Button("Raise"))
                    {
                        ((IEvent) target).Raise();
                    }
                }
                finally
                {
                    GUI.enabled = prev;
                }
            }
        }
    }
}