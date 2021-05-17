using UnityEngine;

namespace LDJam48.PlayerState
{
    public abstract class PlayerState : ScriptableObject
    {
        protected StateMachine _machine;

        public virtual string Name => GetType().Name;

        public void SetMachine(StateMachine machine)
        {
            _machine = machine;
        }

        public virtual PlayerState OnEnter() => null;

        public virtual void OnExit()
        {

        }

        public virtual PlayerState TransitionChecks() => null;

        public virtual PlayerState OnUpdate() => null;

        public virtual PlayerState OnLateUpdate() => null;

        public virtual PlayerState OnFixedUpdate() => TransitionChecks();
    }
}