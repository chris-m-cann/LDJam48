using System;
using UnityEditor;
using UnityEngine;
using Util.Events;
using Util.Variable;

namespace Util
{
    [CustomEditor(typeof(Variable<>), editorForChildClasses:true)]
    public class VariableEditor : Editor
    {
        public SerializedProperty propScript;
        public SerializedProperty propValue;
        public SerializedProperty propResetValue;
        public SerializedProperty propIsPeristant;

        private void OnEnable()
        {
            propScript = serializedObject.FindProperty("m_Script");
            propValue = serializedObject.FindProperty("value");
            propIsPeristant = serializedObject.FindProperty("persistValue");
            propResetValue = serializedObject.FindProperty("resetValue");
        }

        public override void OnInspectorGUI()
        {
            using (serializedObject.UpdateScope())
            {
                var prev = GUI.enabled;
                try
                {
                    GUI.enabled = false;
                    EditorGUILayout.PropertyField(propScript);
                }
                finally
                {
                    GUI.enabled = prev;
                }

                EditorGUILayout.PropertyField(propIsPeristant);

                if (propIsPeristant.boolValue || EditorApplication.isPlaying)
                {
                    EditorGUILayout.PropertyField(propValue);
                }


                if (!propIsPeristant.boolValue)
                {
                    EditorGUILayout.PropertyField(propResetValue);
                }
            }
        }
    }
}