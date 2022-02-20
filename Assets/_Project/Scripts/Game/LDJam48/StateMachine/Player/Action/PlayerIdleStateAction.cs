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
        private HealthRush _healthRush;

        private float _g;

        public override void OnAwake(StateMachineBehaviour machine)
        {
            base.OnAwake(machine);

            _rigidbody = machine.GetComponent<Rigidbody2D>();
            _animator= machine.GetComponent<Animator>();
            machine.TryGetComponent(out _healthRush);
        }

        public override void OnStateEnter()
        {
            base.OnStateEnter();
            _rigidbody.velocity = Vector2.zero;
            _animator.Play(_source.Anim);
            _g = _rigidbody.gravityScale;
            _rigidbody.gravityScale = 0;
            _healthRush.Reset();
        }

        public override void OnStateExit()
        {
            base.OnStateExit();
            _rigidbody.gravityScale = _g;
        }
    }
    
    // on enter from falling or sliding -> play particles + sound + particles
    // condition: dash pressed
}