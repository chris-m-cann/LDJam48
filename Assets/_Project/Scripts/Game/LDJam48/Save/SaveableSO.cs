using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace LDJam48.Save
{
    public abstract class SaveableSO : ScriptableObject
    {
        [OnValueChanged("SetDefaultKey", includeChildren:true, InvokeOnInitialize = true)]
        public string Key;
        public event Action OnRequestSave;
        public event Action OnRequestLoad;
        public event Action OnLoadComplete;

        public abstract ISaveable GetSaveable();
        public abstract void SetSaveable(ISaveable other);

        [HorizontalGroup("Buttons"), Button("Save")]
        public void RequestSave() => OnRequestSave?.Invoke();
        [HorizontalGroup("Buttons"), Button(Name = "Load")]
        public void RequestLoad() => OnRequestLoad?.Invoke();
        public virtual void LoadComplete() => OnLoadComplete?.Invoke();

        public virtual void Enable()
        {
            
        }
        
        public virtual void Disable()
        {

        }

        private void SetDefaultKey()
        {
            if (String.IsNullOrEmpty(Key))
            {
                Key = name;
            }
        }
    }

    public class SaveableSOT<T> : SaveableSO where T : ISaveable
    {
        [InlineProperty] public T Data;


        public override ISaveable GetSaveable()
        {
            return Data;
        }

        public override void SetSaveable(ISaveable other)
        {
            if (other is T t)
            {
                Data = t;
            }
            else
            {
                Debug.LogError($"Failed to load {name}, provided type was {other.GetType().Name}");
            }
        }
    }
}