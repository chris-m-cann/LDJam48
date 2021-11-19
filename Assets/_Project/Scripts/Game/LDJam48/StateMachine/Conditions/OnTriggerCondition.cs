using System;
using UnityEngine;
using Util.Var.Events;
using Void = Util.Void;

namespace LDJam48.StateMachine.Conditions
{
    [Serializable]
    public class OnTriggerCondition : Condition
    {
        public VoidGameEvent Trigger;
        protected override ICondition BuildRuntimeImpl()
        {
            return new OnTriggerConditionRuntime();
        }
    }

    class OnTriggerConditionRuntime : BaseConditionRuntime<OnTriggerCondition>
    {
        private bool _triggered;
        public override bool Evaluate()
        {
            return _triggered;
        }

        private void SetTrigger(Void v)
        {
            _triggered = true;
        }

        public override void OnStateEnter()
        {
            base.OnStateEnter();
            _source.Trigger.OnEventTrigger += SetTrigger;
            _triggered = false;
        }
        
        public override void OnStateExit()
        {
            base.OnStateExit();
            _source.Trigger.OnEventTrigger -= SetTrigger;
        }
        
    }
}