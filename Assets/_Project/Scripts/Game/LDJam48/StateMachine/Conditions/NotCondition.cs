using System;
using UnityEngine;

namespace LDJam48.StateMachine.Conditions
{
    [Serializable]
    public class NotCondition : Condition
    {
        public Condition condition;
        protected override ICondition BuildRuntimeImpl()
        {
            return new NotConditionRuntime();
        }
    }
    
    public class NotConditionRuntime : BaseConditionRuntime<NotCondition>
    {
        private ICondition _condition;
        
        public override void OnAwake(StateMachineBehaviour machine)
        {
            base.OnAwake(machine);
            _condition = _source.condition.BuildRuntime();
            _condition.OnAwake(machine);
        }
        
        public override void OnStateEnter()
        {
            base.OnStateEnter();
            _condition.OnStateEnter();
        }
        
        public override void OnStateExit()
        {
            base.OnStateExit();
            _condition.OnStateExit();
        }

        public override bool Evaluate()
        {
            return !_condition.Evaluate();
        }
    }
}