using UnityEngine;

namespace LDJam48.StateMachine
{
    public abstract class StateAction : ScriptableObject
    {
        public IStateAction BuildRuntime()
        {
            var runtime = BuildRuntimeImpl();
            runtime.SetSource(this);
            runtime.Name = name;
            
            return runtime;
        }

        protected abstract IStateAction BuildRuntimeImpl();
        
        
        public const string MENU_FOLDER = "Custom/StateMachine/Action/";
    }
    
    public interface IStateAction: IStateMachineRuntimeComponent
    {
        public string Name
        {
            get;
            set;
        }
        void OnUpdate();
        void OnFixedUpdate();
        
        void SetSource(StateAction so);
    }
    
    public abstract class BaseStateActionRuntime<SO> : IStateAction where SO: StateAction
    {
        protected SO _source;
        protected StateMachineBehaviour _machine;


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

        public string Name { get; set; }

        public virtual void OnUpdate()
        {
        }

        public virtual void OnFixedUpdate()
        {
        }
    }
}