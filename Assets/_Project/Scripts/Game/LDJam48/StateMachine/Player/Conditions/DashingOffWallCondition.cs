using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Util;
using Util.Var;
using Util.Var.Events;

namespace LDJam48.StateMachine.Player.Conditions
{
    [Serializable]
    public class DashingOffWallCondition : Condition
    {
        public Vector2EventReference dashPress;

        protected override ICondition BuildRuntimeImpl()
        {
            return new DashingOffWallConditionRuntime();
        }
    }

    public class DashingOffWallConditionRuntime : BaseConditionRuntime<DashingOffWallCondition>
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
            if (_contacts.ContactDetails.IsOnLeftWall)
            {
                _isTriggered = direction.x > 0;
            }
            else if (_contacts.ContactDetails.IsOnRightWall)
            {
                _isTriggered = direction.x < 0;
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