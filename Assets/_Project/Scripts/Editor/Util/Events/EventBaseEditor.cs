using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using Util.Variable;

namespace Util.Events
{
    [CustomEditor(typeof(IEventBase), editorForChildClasses:true)]
    public class EventBaseEditor : Editor
    {
        public override void OnInspectorGUI()
        {

            EditorGUILayout.PropertyField(serializedObject.FindProperty("value"));
            if (GUILayout.Button("Raise"))
            {
                ((IEvent) target).Raise();
            }
        }
    }
}