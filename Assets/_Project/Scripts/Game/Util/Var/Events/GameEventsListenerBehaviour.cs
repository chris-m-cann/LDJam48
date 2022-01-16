using UnityEngine;
using UnityEngine.Events;

namespace Util.Var.Events
{
    public abstract class GameEventsListenerBehaviour<T, TGameEvent> : MonoBehaviour where TGameEvent : EventReferenceBase<T>
    {
        [SerializeField]
        public struct Events
        {
            public TGameEvent gameEvent;

            public UnityEvent<T> onEventRaised;

            public void OnEventRaised(T t)
            {
                onEventRaised?.Invoke(t);
            }
        }

        [SerializeField] private Events[] events;
        

        private void OnEnable()
        {
            if (events == null) return;
            
            foreach (var gameEvent in events)
                gameEvent.gameEvent.OnEventTriggered += gameEvent.OnEventRaised;
        }

        private void OnDisable()
        {
            if (events == null) return;
            foreach (var gameEvent in events)
                gameEvent.gameEvent.OnEventTriggered -= gameEvent.OnEventRaised;
        }
    }
}