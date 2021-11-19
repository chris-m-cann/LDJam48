using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Util;

namespace LDJam48.StateMachine
{
    [Serializable]
    public abstract class Condition
    {
        public string Name => GetType().Name;
        public ICondition BuildRuntime()
        {
            var runtime = BuildRuntimeImpl();
            runtime.SetSource(this);
            return runtime;
        }

        protected abstract ICondition BuildRuntimeImpl();
    }

    public interface ICondition: IStateMachineRuntimeComponent
    {
        string Name { get; }
        bool Evaluate();
        void SetSource(Condition condition);
    }

    [Serializable]
    public struct ConditionPair
    {
        [TypeFilter("GetConditionTypeList")]
        public Condition Condition;
        public LogicalOperator Operator;

        public ConditionPairRuntime BuildRuntime()
        {
            return new ConditionPairRuntime
            {
                Condition = Condition.BuildRuntime(),
                Operator = Operator
            };
        }
        
        
        public IEnumerable<Type> GetConditionTypeList() => TypeEx.GetTypeList<Condition>();
    }

    public class ConditionPairRuntime : IStateMachineRuntimeComponent
    {
        public ICondition Condition;
        public LogicalOperator Operator;
        public void OnAwake(StateMachineBehaviour machine)
        {
            Condition.OnAwake(machine);
        }

        public void OnStateEnter()
        {
            Condition.OnStateEnter();
        }

        public void OnStateExit()
        {
            Condition.OnStateExit();
        }
    }

    public enum LogicalOperator
    {
        And, Or
    }
    
    public abstract class BaseConditionRuntime<SO> : ICondition where SO : Condition
    {
        protected SO _source;
        protected StateMachineBehaviour _machine;

        public string Name => _source.Name;

        public void SetSource(Condition so)
        {
            _source = (SO)so;
        }
        
        public virtual void OnAwake(StateMachineBehaviour machine)
        {
            _machine = machine;
        }

        public virtual void OnStateEnter()
        {
        }

        public virtual void OnStateExit()
        {
        }

        public abstract bool Evaluate();
    }
}