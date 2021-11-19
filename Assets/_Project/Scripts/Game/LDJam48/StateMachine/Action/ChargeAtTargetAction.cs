using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Util.Var;

namespace LDJam48.StateMachine.Action
{
    public class ChargeAtTargetAction : StateAction
    {
        public GameObjectReference Target;
        public float SpeedX = 10f;
        protected override IStateAction BuildRuntimeImpl()
        {
            return new ChargeAtTargetActionRuntime();
        }
    }

    public class ChargeAtTargetActionRuntime : BaseStateActionRuntime<ChargeAtTargetAction>
    {
        private Rigidbody2D _rigidbody;
        public override void OnAwake(StateMachineBehaviour machine)
        {
            base.OnAwake(machine);
            _rigidbody = machine.GetComponent<Rigidbody2D>();
        }

        public override void OnStateEnter()
        {
            base.OnStateEnter();

            _rigidbody.velocity = CalculateInterceptVelocity();
        }

        private Vector2 CalculateInterceptVelocity()
        {
            var deltaPos = _source.Target.Value.transform.position - _machine.transform.position;

            var t = Mathf.Abs(deltaPos.x) / _source.SpeedX;

            var targetRb = _source.Target.Value.GetComponent<Rigidbody2D>();
            var targetDeltaPos = targetRb.velocity.y * t;

            var targetEndpoint = _source.Target.Value.transform.position + targetDeltaPos * Vector3.up;

            var ydiff = targetEndpoint.y - _machine.transform.position.y;
            var yvel = ydiff / t;

            return new Vector2(_source.SpeedX * Mathf.Sign(deltaPos.x), yvel);
        }
    }
}