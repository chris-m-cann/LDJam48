using System;
using UnityEngine;
using Util;

namespace LDJam48.StateMachine.Player.Action
{
    [Serializable]
    public class PlayerBounceStateAction : StateAction
    {
        public float bounceVel = 5;
        public float bounceTime = 0.1f;

        protected override IStateAction BuildRuntimeImpl()
        {
            return new PlayerBounceStateActionRuntime();
        }
    }
    
    public class PlayerBounceStateActionRuntime : BaseStateActionRuntime<PlayerBounceStateAction>
    {
        private Rigidbody2D _rigidbody;

        public override void OnAwake(StateMachineBehaviour machine)
        {
            base.OnAwake(machine);
            machine.SetComponent(ref _rigidbody);
        }

        public override void OnStateEnter()
        {
            base.OnStateEnter();
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _source.bounceVel);
        }
    }
    
    // transition after bounce time
}