using System;
using Util.Var.Events;

namespace Util.Var.Observe
{
    public class ObservableVariable<T> : Variable<T>, IEvent
    {
        public event Action<T> OnValueChanged;

        public event Action<T> OnEventTrigger
        {
            add => OnValueChanged += value;
            remove => OnValueChanged -= value;
        }


        public int ActiveObservers => OnValueChanged == null ? 0 : OnValueChanged.GetInvocationList().Length;

        public override T Value
        {
            get => base.Value;
            set
            {
                if (base.Value?.Equals(value) == true) return;

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

        public void SetAndRaise(T v, bool raiseIfEqual)
        {
            if (raiseIfEqual && Value?.Equals(v) == true)
            {
                Raise(v);
            }
            else
            {
                Value = v;
            }
        }
    }
}