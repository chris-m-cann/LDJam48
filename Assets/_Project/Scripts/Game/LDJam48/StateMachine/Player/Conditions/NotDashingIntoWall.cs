using System;
using UnityEngine;
using Util;
using Util.Var.Events;

namespace LDJam48.StateMachine.Player.Conditions
{
    [Serializable]
    public class NotDashingIntoWall : Condition
    {
        public Vector2EventReference dashPress;
        public LayerMask wallLayers;
        protected override ICondition BuildRuntimeImpl()
        {
            return new NotDashingIntoWallRuntime();
        }
    }

    public class NotDashingIntoWallRuntime : BaseConditionRuntime<NotDashingIntoWall>
    {
        private PlayerContacts _contacts;
        private bool _isTriggered;

        public override void OnAwake(StateMachineBehaviour machine)
        {
            base.OnAwake(machine);

            if (!machine.TryGetComponent(out _contacts))
            {
                Debug.LogError("Player contacts not found!");
            }
        }

        public override void OnStateEnter()
        {
            base.OnStateEnter();

            _isTriggered = false;
            _source.dashPress.OnEventTriggered += OnDashPressed;
        }

        public override void OnStateExit()
        {
            base.OnStateExit();

            _source.dashPress.OnEventTriggered -= OnDashPressed;
        }

        private void OnDashPressed(Vector2 direction)
        {
            if (direction.x > 0)
            {
                var onWall = _contacts.ContactDetails.IsOnRightWall && _source.wallLayers.Contains(_contacts.ContactDetails.RightWallCollider.gameObject.layer);
                _isTriggered = !onWall;
            }
            else if (direction.x < 0)
            {
                var onWall = _contacts.ContactDetails.IsOnLeftWall && _source.wallLayers.Contains(_contacts.ContactDetails.LeftWallCollider.gameObject.layer);
                _isTriggered = !onWall;
            }
            else
            {
                _isTriggered = false;
            }
        }

        public override bool Evaluate()
        {
            return _isTriggered;
        }
    }
}