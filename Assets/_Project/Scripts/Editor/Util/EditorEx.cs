using System;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace Util
{
    public static class EditorEx
    {
        public static bool ModifyObject(this SerializedObject self, Action block)
        {
            bool changed = false;
            try
            {
                self.Update();

                block();
            }
            finally
            {
                changed = self.ApplyModifiedProperties();
            }

            return changed;
        }

        public class ModifyObjectScope : IDisposable
        {
            private SerializedObject _so;

            public ModifyObjectScope(SerializedObject so)
            {
                _so = so;
                _so.Update();
            }

            public void Dispose()
            {
                _so.ApplyModifiedProperties();
            }
        }

        public static IDisposable UpdateScope(this SerializedObject self)
        {
            return new ModifyObjectScope(self);
        }

        
        // if left mouse clicked then remove focus from our properties
        public static void RemoveFocusOnMouseDown(this EditorWindow self)
        {
            if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
            {
                GUI.FocusControl(null);
                self.Repaint();
            }
        }
    }
}