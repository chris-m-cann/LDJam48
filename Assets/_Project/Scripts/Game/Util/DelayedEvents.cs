using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace Util
{
    public class DelayedEvents : MonoBehaviour
    {
        [Serializable]
        public struct DelayedEvent
        {
            public float Delay;
            public UnityEvent Event;
        }
        [SerializeField] private DelayedEvent[] events;


        public void Invoke(int idx)
        {
            if (idx > -1 && idx < events.Length)
            {
                var e = events[idx];
                this.ExecuteAfter(e.Delay, () => e.Event?.Invoke());
            }

        }

    }
}