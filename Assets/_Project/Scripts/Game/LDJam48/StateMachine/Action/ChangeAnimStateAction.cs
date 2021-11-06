using UnityEngine;

namespace LDJam48.StateMachine.Action
{
    [CreateAssetMenu(menuName = "Custom/StateMachine/Action/ChangeAnimState")]
    public class ChangeAnimStateAction : StateAction
    {
        public StateMachineCallback When = StateMachineCallback.StateEnter;
        public string AnimState;
        
        public override IStateAction BuildRuntimeImpl()
        {
            return new ChangeAnimStateActionRuntime();
        } }

    public class ChangeAnimStateActionRuntime : BaseStateActionRuntime<ChangeAnimStateAction>
    {
        private Animator _animator;
        private int _hashedAnim;
        
        private void ChangeAnimState()
        {
            _animator.Play(_hashedAnim);
        }
        
        public override void OnAwake(StateMachineBehaviour machine)
        {
            base.OnAwake(machine);
            _animator = _machine.GetComponent<Animator>();
            _hashedAnim = Animator.StringToHash(_source.AnimState);
            
            if (_source.When == StateMachineCallback.Awake)
            {
                ChangeAnimState();
            }
        }

        public override void OnStateEnter()
        {
            base.OnStateEnter();
            if (_source.When == StateMachineCallback.StateEnter)
            {
                ChangeAnimState();
            }
        }

        public override void OnStateExit()
        {
            base.OnStateExit();
            if (_source.When == StateMachineCallback.StateExit)
            {
                ChangeAnimState();
            }
        }
        
        public override void OnUpdate()
        {
            base.OnUpdate();
            if (_source.When == StateMachineCallback.Update)
            {
                ChangeAnimState();
            }
        }
        
        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
            if (_source.When == StateMachineCallback.FixedUpdate)
            {
                ChangeAnimState();
            }
        }
    }
}