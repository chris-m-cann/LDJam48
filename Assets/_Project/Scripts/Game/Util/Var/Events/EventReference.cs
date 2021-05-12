using System;
using UnityEngine;
using Util.Var.Observe;

namespace Util.Var.Events
{
    public abstract class EventReferenceBase<T> : OneOf
    {
        public abstract event Action<T> OnEventTriggered;
        public abstract void Raise(T t);
    }

    public class EventReference<TEvent, TObVar, T> : EventReferenceBase<T> where TEvent : GameEvent<T> where TObVar : ObservableVariable<T>
    {
        [SerializeField] protected TEvent Event;
        [SerializeField] protected  TObVar Variable;

        public override event Action<T> OnEventTriggered
        {
            add
            {
                if (Delimeter == 0) Event.OnEventTrigger += value;
                else Variable.OnValueChanged += value;
            }
            remove
            {
                if (Delimeter == 0) Event.OnEventTrigger -= value;
                else Variable.OnValueChanged -= value;
            }
        }

        public override void Raise(T t)
        {
            if (Delimeter == 0) Event.Raise(t);
            else Variable.Raise(t);
        }

    }
}