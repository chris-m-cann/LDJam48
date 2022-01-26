using System;
using System.Linq;
using System.Reflection;
using LDJam48.Save;
using UnityEngine;
using UnityEngine.Events;
using Util.Var.Observe;

namespace Util.UI
{
    // this needs unifying with the saveable system so Model and Saveable are one thing "DataSource" or something
    // or making it able to take any SO and subscribe if it has a subscribable field or subscribable interface 
    // (similar to saveable)
    public class ModelBinding : MonoBehaviour
    {
        public ModelProvider modelProvider;
        public SaveableSO saveable;

        public Binding<int> intBinding;
        public Binding<string> stringBinding;
        public Binding<Sprite> spriteBinding;
        public Binding<bool> boolBinding;

        [Serializable]
        public struct Binding<T>
        {
            public UnityEvent<T> OnValueChanged;
            public ObservableVariable<T> Observable;

            private FieldInfo _field;
            private SaveableSO _saveable;

            void Update(T value) => OnValueChanged?.Invoke(value);
            void UpdateSaveable() => Update((T)_field.GetValue(_saveable.GetSaveable()));

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
            
            public void Bind(FieldInfo field, SaveableSO saveable)
            {
                _field = field;
                _saveable = saveable;
                if (field.FieldType.HasBase<T>())
                {
                    saveable.OnLoadComplete += UpdateSaveable;
                    UpdateSaveable();
                }
                else if (field.FieldType.HasBase<ObservableVariable<T>>())
                {
                    Observable = (ObservableVariable<T>) field.GetValue(saveable);
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

                if (_saveable != null)
                {
                    _saveable.OnLoadComplete -= UpdateSaveable;
                }
            }
        }
        
        
        [SerializeField] private int fieldIndex;
        [SerializeField] private string fieldName;
        private void OnEnable()
        {
            if (modelProvider != null)
            {
                modelProvider.OnModelChanged += BindUp;
                if (modelProvider.Model != null)
                {
                    BindUp(modelProvider.Model);
                }
            }
            else if (saveable != null)
            {
                BindUpSaveable();
            }
        }

        private void OnDisable()
        {
            if (modelProvider != null)
            {
                modelProvider.OnModelChanged -= BindUp;
            }

            intBinding.UnBind();
            stringBinding.UnBind();
            spriteBinding.UnBind();
            boolBinding.UnBind();
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
            boolBinding.Bind(field, model);
        }
        private void BindUpSaveable()
        {
            // make this look up be a one time thing for each type?
            var fields = GetBindableFields(saveable.GetSaveable().GetType());

            int fieldIdx = FindProperty(fields);
            
            if (fieldIdx == -1) return;


            var field = fields[fieldIdx];

            intBinding.Bind(field, saveable);
            stringBinding.Bind(field, saveable);
            spriteBinding.Bind(field, saveable);
            boolBinding.Bind(field, saveable);
        }

            public FieldInfo[] GetFeilds()
            {
                if (modelProvider != null)
                {
                    return modelProvider.Model.Fields.Value;
                } else if (saveable != null)
                {
                    return GetBindableFields(saveable.GetSaveable().GetType());
                }

                return new FieldInfo[0];
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
            typeof(ObservableStringVariable),
            typeof(bool)
        };
    }
}