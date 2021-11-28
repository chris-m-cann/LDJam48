using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace LDJam48.StateMachine
{
    public abstract class OneShotAction
    {
        public string Name => GetType().Name;
        public IOneShotAction BuildRuntime()
        {
            var runtime = BuildRuntimeImpl();
            runtime.SetSource(this);
            return runtime;
        }
        
        protected abstract IOneShotAction BuildRuntimeImpl();
    }

    public interface IOneShotAction
    {
        public string Name { get; }
        public void OnAwake(StateMachineBehaviour machine);

        public void Execute();

        public void SetSource(OneShotAction source);
    }

    public abstract class BaseOneShotActionRuntime<SO> : IOneShotAction where SO : OneShotAction
    {
        protected SO _source;
        protected StateMachineBehaviour _machine;

        public string Name => _source.Name;

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

    public static class OneShotActions
    {
        public static void Execute(this IEnumerable<IOneShotAction> self)
        {
            foreach (var action in self)
            {
                action.Execute();
            }
        }
    }
}