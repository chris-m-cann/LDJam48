using System;
using Object = UnityEngine.Object;

namespace LDJam48.StateMachine.Action
{
    [Serializable]
    public class DestroyGameObjectAction : OneShotAction
    {
        protected override IOneShotAction BuildRuntimeImpl()
        {
            return new DestroyGameObjectActionRuntime();
        }
    }
    
    public class DestroyGameObjectActionRuntime : BaseOneShotActionRuntime<DestroyGameObjectAction>
    {
        public override void Execute()
        {
            Object.Destroy(_machine.gameObject);
        }
    }
}