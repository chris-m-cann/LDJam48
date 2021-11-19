using System;
using UnityEngine;

namespace LDJam48.StateMachine.Conditions
{
    
    [Serializable]
    public class AfterTimeCondition : Condition
    {
        public float timeout = 1f;
        protected override ICondition BuildRuntimeImpl()
        {
            return new AfterTimeConditionRuntime();
        }
    }
    
    public class AfterTimeConditionRuntime : BaseConditionRuntime<AfterTimeCondition>
    {
        private float _timeoutTime;

        public override void OnStateEnter()
        {
            base.OnStateEnter();
            _timeoutTime = Time.time + _source.timeout;
        }

        public override bool Evaluate()
        {
            return Time.time > _timeoutTime;
        }
    }
}