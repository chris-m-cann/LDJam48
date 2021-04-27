using System;
using UnityEngine;

namespace Util.Variable
{
    public class Variable<T>: ScriptableObject
    {
        [NonSerialized] private T value;
        [SerializeField] protected T resetValue;

        public virtual T Value {
            get => value;
            set => this.value = value;
        }

        private void OnEnable()
        {
            value = resetValue;
        }

        private void OnDisable()
        {
            value = resetValue;
        }

        public void Set(T newValue) => Value = newValue;
        public T Get() => Value;

        public void Reset()
        {
            Value = resetValue;
        }
    }

    public class PersistantVariable<T> : Variable<T>
    {
        public override T Value { get => resetValue; set => resetValue = value; }
    }
}