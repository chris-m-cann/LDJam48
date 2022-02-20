using System;
using UnityEngine;
using Util;
using Util.Var;

namespace LDJam48.StateMachine.Conditions
{
    [Serializable]
    public class CompareTimeDiff : Condition
    {
        public FloatReference Timestamp;
        public CompareOp Comparison;
        public FloatReference Diff;
        public bool consumeOnTrue = true;
        protected override ICondition BuildRuntimeImpl()
        {
            return new CompareTimeDiffRuntime();
        }
    }

    public class CompareTimeDiffRuntime : BaseConditionRuntime<CompareTimeDiff>
    {
        private bool _triggered;

        public override void OnStateEnter()
        {
            base.OnStateEnter();
            _triggered = false;
        }

        public override bool Evaluate()
        {
            if (_source.consumeOnTrue && _triggered) return false;
            var diff = Time.time - _source.Timestamp.Value;
            _triggered = CompareOpEx.Compare(_source.Comparison, diff, _source.Diff.Value);
            return _triggered;
        }
    }
}