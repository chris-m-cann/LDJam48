using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace LDJam48.StateMachine.Player.Conditions
{
    [Serializable]
    public class CompareVelocityCondition : Condition
    {
        public bool CompareX;
        [EnableIf(nameof(CompareX))]
        public CompareOp XOp;
        [EnableIf(nameof(CompareX))] 
        public float CompareXTo;
        [Space]
        public bool CompareY;
        [EnableIf(nameof(CompareY))]
        public CompareOp YOp;
        [EnableIf(nameof(CompareY))] 
        public float CompareYTo;
        protected override ICondition BuildRuntimeImpl()
        {
            return new CompareVelocityConditionRuntime();
        }
    }

    public enum CompareOp
    {
        LessThan = -1, Equals = 0, GreaterThan = 1
    }

    public class CompareVelocityConditionRuntime : BaseConditionRuntime<CompareVelocityCondition>
    {
        private Rigidbody2D _rigidbody;

        public override void OnAwake(StateMachineBehaviour machine)
        {
            base.OnAwake(machine);
            _rigidbody = machine.GetComponent<Rigidbody2D>();
        }

        public override bool Evaluate()
        {
            bool r = true;

            if (_source.CompareX)
            {
                r = r && (_rigidbody.velocity.x.CompareTo(_source.CompareXTo) == (int)_source.XOp);
            }
            
            if (_source.CompareY)
            {
                r = r && (_rigidbody.velocity.y.CompareTo(_source.CompareYTo) == (int)_source.YOp);
            }

            return r;
        }
    }
}