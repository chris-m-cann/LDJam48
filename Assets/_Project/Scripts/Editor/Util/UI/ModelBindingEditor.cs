using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using Util.Var.Events;
using Util.Var.Observe;

namespace Util.UI
{
    [CustomEditor(typeof(ModelBinding))]
    public class ModelBindingEditor : Editor
    {
        private SerializedProperty _propScript;
        private SerializedProperty _propModel;
        private SerializedProperty _propSaveable;
        private SerializedProperty _propIntEvent;
        private SerializedProperty _propStringEvent;
        private SerializedProperty _propSpriteEvent;
        private SerializedProperty _propBoolEvent;
        private SerializedProperty _propFloatEvent;
        private SerializedProperty _propFieldIndex;
        private SerializedProperty _propFieldName;

        private void OnEnable()
        {
            _propScript = serializedObject.FindProperty("m_Script");
            _propModel = serializedObject.FindProperty(nameof(ModelBinding.modelProvider));
            _propSaveable = serializedObject.FindProperty(nameof(ModelBinding.saveable));
            _propIntEvent = serializedObject.FindProperty(nameof(ModelBinding.intBinding)).FindPropertyRelative(nameof(ModelBinding.Binding<int>.OnValueChanged));
            _propStringEvent = serializedObject.FindProperty(nameof(ModelBinding.stringBinding)).FindPropertyRelative(nameof(ModelBinding.Binding<int>.OnValueChanged));
            _propSpriteEvent = serializedObject.FindProperty(nameof(ModelBinding.spriteBinding)).FindPropertyRelative(nameof(ModelBinding.Binding<int>.OnValueChanged));
            _propBoolEvent = serializedObject.FindProperty(nameof(ModelBinding.boolBinding)).FindPropertyRelative(nameof(ModelBinding.Binding<int>.OnValueChanged));
            _propFloatEvent = serializedObject.FindProperty(nameof(ModelBinding.floatBinding)).FindPropertyRelative(nameof(ModelBinding.Binding<int>.OnValueChanged));
            _propFieldIndex = serializedObject.FindProperty("fieldIndex");
            _propFieldName = serializedObject.FindProperty("fieldName");
        }

        public override void OnInspectorGUI()
        {
            using (serializedObject.UpdateScope())
            {

                DisplayScriptField();
                
                EditorGUILayout.PropertyField(_propModel);
                EditorGUILayout.PropertyField(_propSaveable);
                
                var field = DisplayPropertyPopup();
                
                if (field == null) return;

                DisplayEventIfRequired<int>(field, _propIntEvent);
                DisplayEventIfRequired<string>(field, _propStringEvent);
                DisplayEventIfRequired<Sprite>(field, _propSpriteEvent);
                DisplayEventIfRequired<bool>(field, _propBoolEvent);
                DisplayEventIfRequired<float>(field, _propFloatEvent);
            }
        }

        private void DisplayScriptField()
        {
            // make sure we display the script but cant let you change it
            var prev = GUI.enabled;
            try
            {
                GUI.enabled = false;
                EditorGUILayout.PropertyField(_propScript);
            }
            finally
            {
                GUI.enabled = prev;
            }
        }

        private FieldInfo DisplayPropertyPopup()
        {
            var t = (ModelBinding)target;
            var fields = t.GetFeilds();
            var options = fields.Select(it => it.Name).ToArray();

            var propIdx = t.FindProperty(fields);

            var newProp = EditorGUILayout.Popup("Property", propIdx, options);

            if (fields.Length == 0) return null;


            var field = fields[newProp];


            // if field has changed let binding know
            if (newProp != propIdx)
            {
                _propFieldIndex.intValue = newProp;
                _propFieldName.stringValue = field.Name;
            }

            return field;
        }

        

        private void DisplayEventIfRequired<T>(FieldInfo field, SerializedProperty property)
        {
            if (field.FieldType == typeof(T) || field.FieldType.HasBase<ObservableVariable<T>>())
            {
                EditorGUILayout.PropertyField(property);
            }
        }

        // todo(chris) add in direct binding to a field/ function rather than unity events?
        private void DisplayOutputPopup(ModelBinding binding)
        {
            var options = new List<string>();
            foreach ( var component in binding.gameObject.GetComponents<MonoBehaviour>())
            {
                foreach (var field in component.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
                {
                    options.Add($"{component.GetType().Name}/{field.Name}");
                }

            }

            EditorGUILayout.Popup("output", 0, options.ToArray());
        }
    }
}