using System;
using UnityEngine;
using Util;
using Util.Var.Events;

namespace LDJam48.StateMachine.Player.Action
{
    [Serializable]
    public class PlayerIdleStateAction : StateAction
    {
        public string Anim = "player_idle";


        protected override IStateAction BuildRuntimeImpl()
        {
            return new PlayerIdleStateActionRuntime();
        }
    }

    public class PlayerIdleStateActionRuntime : BaseStateActionRuntime<PlayerIdleStateAction>
    {
        private Rigidbody2D _rigidbody;
        private Animator _animator;

        public override void OnAwake(StateMachineBehaviour machine)
        {
            base.OnAwake(machine);

            _rigidbody = machine.GetComponent<Rigidbody2D>();
            _animator= machine.GetComponent<Animator>();
        }

        public override void OnStateEnter()
        {
            base.OnStateEnter();
            _rigidbody.velocity = Vector2.zero;
            _animator.Play(_source.Anim);
        }
    }
    
    // on enter from falling or sliding -> play particles + sound + particles
    // condition: dash pressed
}