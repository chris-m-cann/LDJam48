using System;
using UnityEngine;

namespace Util.Var
{
    public class Variable<T>: ScriptableObject
    {
        [SerializeField] protected T value;
        [SerializeField] protected bool persistValue;
        [SerializeField] protected T resetValue;

        public virtual T Value {
            get => value;
            set
            {
                this.value = value;
            }
        }

        private void OnEnable()
        {
            if (!persistValue)
            {
                value = resetValue;
            }
        }

        private void OnDisable()
        {
            if (!persistValue)
            {
                value = resetValue;
            }
        }

        public void Set(T newValue) => Value = newValue;
        public T Get() => Value;

        public void Reset()
        {
            Value = resetValue;
        }
    }
}