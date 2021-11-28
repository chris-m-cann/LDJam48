using System;
using Util.Var;

namespace LDJam48.StateMachine.Player.Action
{
    [Serializable]
    public class SetBooleanAction : OneShotAction
    {
        public BoolVariable Var;
        public bool Value;
        protected override IOneShotAction BuildRuntimeImpl()
        {
            return new SetBooleanActionRuntime();
        }
    }
    
    public class SetBooleanActionRuntime : BaseOneShotActionRuntime<SetBooleanAction>
    {
        public override void Execute()
        {
            _source.Var.Value = _source.Value;
        }
    }
}