using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace LDJam48.StateMachine
{
    public abstract class OneShotAction : ScriptableObject
    {
        public IOneShotAction BuildRuntime()
        {
            var runtime = BuildRuntimeImpl();
            runtime.SetSource(this);
            return runtime;
        }
        
        protected abstract IOneShotAction BuildRuntimeImpl();

        public const string MENU_FOLDER = "Custom/StateMachine/OneShotAction/";
    }

    public interface IOneShotAction
    {
        public void OnAwake(StateMachineBehaviour machine);

        public void Execute();

        public void SetSource(OneShotAction source);
    }

    public abstract class BaseOneShotActionRuntime<SO> : IOneShotAction where SO : OneShotAction
    {
        protected SO _source;
        protected StateMachineBehaviour _machine;

        public virtual void OnAwake(StateMachineBehaviour machine)
        {
            _machine = machine;
        }

        public void SetSource(OneShotAction source)
        {
            _source = (SO)source;
        }

        public abstract void Execute();
    }
}