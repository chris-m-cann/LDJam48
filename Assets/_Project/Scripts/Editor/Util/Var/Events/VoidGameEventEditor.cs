using UnityEditor;
using UnityEngine;

namespace Util.Var.Events
{
    [CustomEditor(typeof(VoidGameEvent))]
    [CanEditMultipleObjects]
    public class VoidGameEventEditor : Editor
    {

        public override void OnInspectorGUI()
        {
            var prev = GUI.enabled;
            try
            {
                GUI.enabled = false;
                EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Script"));

                GUI.enabled = EditorApplication.isPlaying;
                if (GUILayout.Button("Raise"))
                {
                    ((VoidGameEvent) target).Raise();
                }
            }
            finally
            {
                GUI.enabled = prev;
            }
        }
    }
}