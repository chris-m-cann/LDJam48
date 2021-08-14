using System;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using Util.Var.Observe;

namespace Util.UI
{
    public class ModelBinding : MonoBehaviour
    {
        public ModelProvider modelProvider;

        public Binding<int> intBinding;
        public Binding<string> stringBinding;
        public Binding<Sprite> spriteBinding;

        [Serializable]
        public struct Binding<T>
        {
            public UnityEvent<T> OnValueChanged;
            public ObservableVariable<T> Observable;

            void Update(T value) => OnValueChanged?.Invoke(value);

            public void Bind(FieldInfo field, Model model)
            {
                if (field.FieldType.HasBase<T>())
                {
                    Update((T)field.GetValue(model));
                }
                else if (field.FieldType.HasBase<ObservableVariable<T>>())
                {
                    Observable = (ObservableVariable<T>) field.GetValue(model);
                    Observable.OnValueChanged += Update;
                    Update(Observable.Value);
                }
            }

            public void UnBind()
            {
                if (Observable != null)
                {
                    Observable.OnValueChanged -= Update;
                }
            }
        }
        
        
        [SerializeField] private int fieldIndex;
        [SerializeField] private string fieldName;
        private void OnEnable()
        {
            modelProvider.OnModelChanged += BindUp;
            if (modelProvider.Model != null)
            {
                BindUp(modelProvider.Model);
            }
        }

        private void OnDisable()
        {
            modelProvider.OnModelChanged -= BindUp;
            intBinding.UnBind();
            stringBinding.UnBind();
            spriteBinding.UnBind();
        }

        private void BindUp(Model model)
        {
            var fields = model.Fields.Value;

            int fieldIdx = FindProperty(fields);
            
            if (fieldIdx == -1) return;


            var field = fields[fieldIdx];

            intBinding.Bind(field, model);
            stringBinding.Bind(field, model);
            spriteBinding.Bind(field, model);
        }


        public int FindProperty(FieldInfo[] fields)
        {
            if (fields.Length > fieldIndex)
            {
                // if the field at fieldIndex is the same as fieldName then we are good
                // else try and find it by name
                // if you can the update the index else update then name and presume it was just a rename
                if (fields[fieldIndex].Name != fieldName)
                {
                    var idx = Array.FindIndex(fields, it => it.Name == fieldName);
                    if (idx != -1)
                    {
                        fieldIndex = idx;
                    }
                    else
                    {
                        fieldName = fields[fieldIndex].Name; 
                    }
                }
            }
            else
            {
                var idx = Array.FindIndex(fields, it => it.Name == fieldName);
                if (idx == -1)
                {
                    Debug.LogError($"Could not find property '{fieldName}' in model '{modelProvider.Model.name}'");
                    return -1;
                }

                fieldIndex = idx;
            }

            return fieldIndex;
        }
        

        public static FieldInfo[] GetBindableFields(Type type)
        {
            return type
                .GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
                .Where(field => SupportedTypes.Any(it => field.FieldType.HasBase(it))).ToArray();
        }

        private static readonly Type[] SupportedTypes = {
            typeof(int),
            typeof(string),
            typeof(Sprite),
            typeof(ObservableIntVariable),
            typeof(ObservableStringVariable)
        };
    }
}