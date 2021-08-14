using UnityEditor;
using UnityEngine;
using Util.UI;

namespace Util
{
    [CustomPropertyDrawer(typeof(NestedAttribute))]
    public class NestedPropertyDraw : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property,
            GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            try
            {

                if (property.objectReferenceValue != null)
                {
                    EditorGUI.PropertyField(position, property, label, true);
                    return;
                }

                EditorGUI.LabelField(position, label);

                var fieldPos = position;
                fieldPos.width -=  20;

                EditorGUI.PropertyField(fieldPos, property);

                var buttonPos = fieldPos;
                
                buttonPos.x += fieldPos.width;
                buttonPos.width = 20;

                if (GUI.Button(buttonPos, "+"))
                {
                    var path = AssetDatabase.GetAssetPath(property.serializedObject.targetObject);
                    NestedAssetPostProcessor.AddNestedAssets(path, property.serializedObject.targetObject);
                }
            }
            finally
            {
                EditorGUI.EndProperty();   
            }
        }
    }
}