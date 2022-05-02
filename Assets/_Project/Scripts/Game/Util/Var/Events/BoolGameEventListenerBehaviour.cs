using UnityEngine.Events;

namespace Util.Var.Events
{
    using UnityEngine;
    
    public sealed class BoolGameEventListenerBehaviour : GameEventListenerBehaviour<bool, BoolEventReference>
    {
        [SerializeField] private UnityEvent onTrue;
        [SerializeField] private UnityEvent onFalse;

        public override void OnEventRaised(bool t)
        {
            base.OnEventRaised(t);
            if (t)
            {
                onTrue?.Invoke();
            }
            else
            {
                onFalse?.Invoke();
            }
        }
    }
}
