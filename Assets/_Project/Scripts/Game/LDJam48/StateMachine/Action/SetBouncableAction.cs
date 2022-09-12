using UnityEngine;

namespace LDJam48.StateMachine.Action
{
    public class SetBouncableAction : StateAction
    {
        public bool EnabledOnEnter;
        public bool EnabledOnExit;
        protected override IStateAction BuildRuntimeImpl()
        {
            return new SetBouncableActionRuntime();
        }
    
    }

    public class SetBouncableActionRuntime : BaseStateActionRuntime<SetBouncableAction>
    {
        private BounceableDamaging _behaviour;

        public override void OnAwake(StateMachineBehaviour machine)
        {
            base.OnAwake(machine);

            if (!machine.TryGetComponent(out _behaviour))
            {
                Debug.LogError($"{machine.gameObject.name}, BounceableDamaging not found");
            }
        }

        public override void OnStateEnter()
        {
            base.OnStateEnter();
            if (_behaviour == null) return;

            _behaviour.UseSafeAngles = _source.EnabledOnEnter;
        }

        public override void OnStateExit()
        {
            base.OnStateExit();
            if (_behaviour == null) return;
            _behaviour.UseSafeAngles = _source.EnabledOnExit;
        }
    }
}