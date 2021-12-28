using System;
using LDJam48.StateMachine.Player.Conditions;
using Sirenix.OdinInspector;
using Util;

namespace LDJam48.StateMachine.Conditions
{
    [Serializable]
    public class OnStateCompletesItselfCondition : Condition
    {
        public bool anyValue;
        [ShowIf("@!anyValue")]
        public CompareOp comparison;
        [ShowIf("@!anyValue")]
        public int compareTo;
        protected override ICondition BuildRuntimeImpl()
        {
            return new OnStateCompletesItselfConditionRuntime();
        }
    }

    class OnStateCompletesItselfConditionRuntime : BaseConditionRuntime<OnStateCompletesItselfCondition>
    {
        private bool _isSet;
        
        public override void OnStateEnter()
        {
            base.OnStateEnter();
            _isSet = false;

            _machine.StateCompletedItself += OnTrigger;
        }

        public override void OnStateExit()
        {
            base.OnStateExit();
            _machine.StateCompletedItself -= OnTrigger;
        }

        public override bool Evaluate()
        {
            return _isSet;
        }

        private void OnTrigger(int exitVal)
        {
            if (_source.anyValue)
            {
                _isSet = true;
                return;
            }

            _isSet = _source.comparison.Compare(exitVal, _source.compareTo);
        }
    }
}