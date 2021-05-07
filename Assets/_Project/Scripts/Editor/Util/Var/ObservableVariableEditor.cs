using UnityEditor;
using UnityEngine;
using Util.Events;
using Util.Var;
using Util.Var.Events;
using Util.Var.Observe;

namespace Util
{
    [CustomEditor(typeof(ObservableVariable<>), editorForChildClasses:true)]
    public class ObservableVariableEditor : VariableEditor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

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