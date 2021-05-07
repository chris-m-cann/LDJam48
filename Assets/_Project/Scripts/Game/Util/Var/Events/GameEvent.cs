using System;
using UnityEngine;

namespace Util.Var.Events
{
    public interface IEvent
    {
        void Raise();
    }

    public class GameEvent<T> : ScriptableObject, IEvent
    {
        public event Action<T> OnEventTrigger;


#if UNITY_EDITOR
        public T Value;
        public void Raise() => Raise(Value);
#endif
        public void Raise(T t) => OnEventTrigger?.Invoke(t);
    }
}