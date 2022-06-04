using System;
using UnityEngine;

namespace LDJam48.StateMachine.Player.Conditions
{
    
    [Serializable]
    public class AfterMinDistanceCondition : Condition
    {
        public float distance = 1f;
        protected override ICondition BuildRuntimeImpl()
        {
            return new AfterMinDistanceConditionRuntime();
        }
    }
    
    public class AfterMinDistanceConditionRuntime : BaseConditionRuntime<AfterMinDistanceCondition>
    {
        private float _startX;

        public override void OnStateEnter()
        {
            base.OnStateEnter();
            _startX = _machine.transform.position.x;
            if (_machine.debugLogs)
            {
                Debug.Log($"AfterMinDistanceCondition: initial pos = {_startX}, _minDistance = {_source.distance}");
            }
        }

        public override bool Evaluate()
        {
            var r = Mathf.Abs(_machine.transform.position.x - _startX) > _source.distance;
            
            if (_machine.debugLogs && r)
            {
                Debug.Log($"AfterMinDistanceCondition: initial pos = {_startX}, _minDistance = {_source.distance}, eval ={r}");
            }

            return r;
        }
    }
}