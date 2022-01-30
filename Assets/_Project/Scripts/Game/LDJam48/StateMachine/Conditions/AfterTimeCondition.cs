using System;
using UnityEngine;

namespace LDJam48.StateMachine.Conditions
{
    
    [Serializable]
    public class AfterTimeCondition : Condition
    {
        public float timeout = 1f;
        public bool debug;
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
            if (_source.debug)
            {
                Debug.Log($"AfterTimeConditionRuntime: time = {Time.time}, _timeoutTime = {_timeoutTime}");
            }
        }

        public override bool Evaluate()
        {
            var r = Time.time > _timeoutTime;
            
            if (_source.debug)
            {
                Debug.Log($"AfterTimeConditionRuntime: time = {Time.time}, _timeoutTime = {_timeoutTime}, eval = {r}");
            }

            return r;
        }
    }
}