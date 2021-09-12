using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace Util
{
    [CustomPropertyDrawer(typeof(OneOf), true)]
    public class OneOfPropertyDraw : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property,
            GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var delimeter = property.FindPropertyRelative("Delimeter");

            var properties = property.GetChildren().Where(it => !SerializedProperty.EqualContents(it, delimeter))
                .ToArray();
            var list = properties.Select(it => it.displayName);


            EditorGUI.LabelField(position, label);


            var fieldPos = position;
            fieldPos.x += EditorGUIUtility.labelWidth;
            fieldPos.width -= EditorGUIUtility.labelWidth;

            var popUpPos = fieldPos;
            popUpPos.width = 15;

            var popupStyle = new GUIStyle(GUI.skin.GetStyle("PaneOptions"));
            popupStyle.imagePosition = ImagePosition.ImageOnly;
            delimeter.intValue = EditorGUI.Popup(popUpPos, delimeter.intValue, list.ToArray(), popupStyle);

            fieldPos.x += popUpPos.width + 15;
            fieldPos.width -= popUpPos.width + 15;
            fieldPos.height = EditorGUI.GetPropertyHeight(properties[delimeter.intValue], true);

            EditorGUI.PropertyField(fieldPos, properties[delimeter.intValue], GUIContent.none);

            EditorGUI.EndProperty();
        }

        private static List<string> GetPropertyNames(SerializedProperty property)
        {
            var list = new List<string>();

            var it = property.Copy().GetEnumerator();

            while (it.MoveNext())
            {
                if (it.Current is SerializedProperty sp)
                {
                    if (sp.name != "Delimeter")
                    {
                        list.Add(sp.name);
                    }
                }
            }

            return list;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var delimeter = property.FindPropertyRelative("Delimeter");

            var properties = property.GetChildren().Where(it => !SerializedProperty.EqualContents(it, delimeter))
                .ToArray();
            return EditorGUI.GetPropertyHeight(properties[delimeter.intValue], true);
        }
    }
}