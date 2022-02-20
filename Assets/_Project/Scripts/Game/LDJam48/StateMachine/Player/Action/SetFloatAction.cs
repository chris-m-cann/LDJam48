using System;
using Util.Var;

namespace LDJam48.StateMachine.Player.Action
{
    namespace LDJam48.StateMachine.Player.Action
    {
        [Serializable]
        public class SetFloatAction : OneShotAction
        {
            public FloatVariable Var;
            public float Value;
            protected override IOneShotAction BuildRuntimeImpl()
            {
                return new SetFloatActionRuntime();
            }
        }
    
        public class SetFloatActionRuntime : BaseOneShotActionRuntime<SetFloatAction>
        {
            public override void Execute()
            {
                _source.Var.Value = _source.Value;
            }
        }
    }
}