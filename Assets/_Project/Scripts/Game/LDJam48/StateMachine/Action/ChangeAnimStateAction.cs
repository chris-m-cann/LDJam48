using UnityEngine;

namespace LDJam48.StateMachine.Action
{
    [CreateAssetMenu(menuName = OneShotAction.MENU_FOLDER + "ChangeAnimState")]
    public class ChangeAnimStateAction : OneShotAction
    {
        public string AnimState;
        
        protected override IOneShotAction BuildRuntimeImpl()
        {
            return new ChangeAnimStateActionRuntime();
        } 
    }

    public class ChangeAnimStateActionRuntime : BaseOneShotActionRuntime<ChangeAnimStateAction>
    {
        private Animator _animator;
        private int _hashedAnim;
        
        public override void OnAwake(StateMachineBehaviour machine)
        {
            base.OnAwake(machine);
            _animator = _machine.GetComponent<Animator>();
            _hashedAnim = Animator.StringToHash(_source.AnimState);
        }

        public override void Execute()
        {
            _animator.Play(_hashedAnim);
        }
    }
}