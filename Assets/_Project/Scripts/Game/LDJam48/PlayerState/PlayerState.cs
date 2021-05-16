using UnityEngine;

namespace LDJam48.PlayerState
{
    public abstract class PlayerState : ScriptableObject
    {
        protected StateMachine _machine;

        public virtual string Name => GetType().Name;

        public virtual void OnEnter(StateMachine machine)
        {
            _machine = machine;

            TransitionChecks();
        }

        public virtual void OnExit()
        {

        }

        public virtual void TransitionChecks()
        {

        }

        public virtual void OnUpdate()
        {

        }

        public virtual void OnLateUpdate()
        {

        }

        public virtual void OnFixedUpdate()
        {
            TransitionChecks();
        }
    }
}