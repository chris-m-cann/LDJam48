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
            var tmp = _triggered;
            _triggered = false;
            return tmp;
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
    
    public class OnTriggerConditionT<T, TEvent> : Condition where TEvent : GameEvent<T>
    {
        public TEvent Trigger;
        protected override ICondition BuildRuntimeImpl()
        {
            return new OnTriggerConditionRuntimeT<T, TEvent>();
        }
    }

    [Serializable]
    public class OnVoidTriggerCondition : OnTriggerConditionT<Void, VoidGameEvent>
    {
        
    }
    
    
    [Serializable]
    public class OnVector2TriggerCondition : OnTriggerConditionT<Vector2, Vector2GameEvent>
    {
        
    }

    class OnTriggerConditionRuntimeT<T, TEvent> : BaseConditionRuntime<OnTriggerConditionT<T, TEvent>> where TEvent : GameEvent<T>
    {
        private bool _triggered;
        public override bool Evaluate()
        {
            var tmp = _triggered;
            _triggered = false;
            return tmp;
        }

        private void SetTrigger(T v)
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