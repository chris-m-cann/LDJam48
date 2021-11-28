using System;
using UnityEngine;

namespace Util.Var.Events
{
    public interface IEvent
    {
        void Raise();
    }

    public class GameEvent<T> : ScriptableObject
        , IEvent
    {
        public event Action<T> OnEventTrigger;


        public T Value;
        public void Raise() => Raise(Value);
        public void Raise(T t) => OnEventTrigger?.Invoke(t);
    }
}