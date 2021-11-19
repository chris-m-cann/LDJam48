using System;
using UnityEngine;

namespace LDJam48.StateMachine
{
    [Serializable]
    public abstract class StateAction
    {
        public string Name => GetType().Name;
        public IStateAction BuildRuntime()
        {
            var runtime = BuildRuntimeImpl();
            runtime.SetSource(this);
            
            return runtime;
        }

        protected abstract IStateAction BuildRuntimeImpl();
    }
    
    public interface IStateAction: IStateMachineRuntimeComponent
    {
        string Name { get; }
        void OnUpdate();
        void OnFixedUpdate();
        
        void SetSource(StateAction so);
    }
    
    public abstract class BaseStateActionRuntime<SO> : IStateAction where SO: StateAction
    {
        protected SO _source;
        protected StateMachineBehaviour _machine;

        public string Name
        {
            get => _source.Name;
        }


        public void SetSource(StateAction so)
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

        public virtual void OnUpdate()
        {
        }

        public virtual void OnFixedUpdate()
        {
        }
    }
}