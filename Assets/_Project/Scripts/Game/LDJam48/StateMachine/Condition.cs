using System;
using UnityEngine;

namespace LDJam48.StateMachine
{
    public abstract class Condition : ScriptableObject
    {
        public ICondition BuildRuntime()
        {
            var runtime = BuildRuntimeImpl();
            runtime.SetSource(this);
            runtime.Name = name;
            return runtime;
        }

        protected abstract ICondition BuildRuntimeImpl();
        
        public const string MENU_FOLDER = "Custom/StateMachine/Condition/";
    }

    public interface ICondition: IStateMachineRuntimeComponent
    {
        string Name { get; set; }
        bool Evaluate();
        void SetSource(Condition condition);
    }

    [Serializable]
    public struct ConditionPair
    {
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

        public string Name { get; set; }
        public abstract bool Evaluate();
    }
}