using System;
using Util.Var.Events;

namespace LDJam48.StateMachine.Player.Action
{
    public abstract class RaiseEventAction<T, TEventReference> : OneShotAction where TEventReference : EventReferenceBase<T>
    {
        public TEventReference EventReference;
        public T ValueToRaise;
    }
    
    [Serializable]
    public class RaiseShakeEventAction : RaiseEventAction<ShakeDefinition, ShakeDefinitionEventReference>
    {
        protected override IOneShotAction BuildRuntimeImpl()
        {
            return new RaiseShakeEventActionRuntime();
        }
    }

    public class RaiseShakeEventActionRuntime : BaseOneShotActionRuntime<RaiseShakeEventAction>
    {
        public override void Execute()
        {
            _source.EventReference.Raise(_source.ValueToRaise);
        }
    }
}