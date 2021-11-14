using UnityEngine;

namespace LDJam48.StateMachine.Action
{
    [CreateAssetMenu(menuName = MENU_FOLDER + "DestroyGameObject")]
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