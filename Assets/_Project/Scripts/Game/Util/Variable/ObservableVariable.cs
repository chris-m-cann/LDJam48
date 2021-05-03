using System;
using UnityEngine;

namespace Util.Variable
{
    public interface IEvent
    {
        void Raise();
    }

    public class ObservableVariable<T> : Variable<T>, IEvent
    {
        public event Action<T> OnValueChanged;

        public event Action<T> OnEventTrigger
        {
            add => OnValueChanged += value;
            remove => OnValueChanged -= value;
        }

        public override T Value
        {
            get => base.Value;
            set
            {
                if (base.Value.Equals(value)) return;

                base.Value = value;
                Raise(value);
            }
        }

        public void Raise()
        {
            Raise(Value);
        }

        public void Raise(T t)
        {
            OnValueChanged?.Invoke(t);
        }
    }
}