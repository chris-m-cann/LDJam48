using UnityEngine;
using UnityEngine.Events;

namespace LDJam48
{
    public class AnimationEventDispatcher : MonoBehaviour
    {
        [SerializeField] private UnityEvent[] unityEvents;

        public void TriggerEvent(int idx)
        {
            if (idx >= 0 && idx < unityEvents.Length)
            {
                unityEvents[idx]?.Invoke();
            }
        }

    }
}