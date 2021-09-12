using UnityEditor;
using UnityEngine;

namespace Util
{
    [CustomPropertyDrawer(typeof(Pair<,>))]
    public class PairDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property,
            GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var it = property.FindPropertyRelative("Item1");

            var fieldPos = position;

            EditorGUI.MultiPropertyField(fieldPos, new []{new GUIContent("1st"), new GUIContent("2nd")}, it, label);
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var height1 = EditorGUI.GetPropertyHeight(property.FindPropertyRelative("Item1"));
            var height2 = EditorGUI.GetPropertyHeight(property.FindPropertyRelative("Item2"));
            return Mathf.Max(height1, height2);
        }
    }
}