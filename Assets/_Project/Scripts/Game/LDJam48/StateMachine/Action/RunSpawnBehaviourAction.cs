using UnityEngine;

namespace LDJam48.StateMachine.Action
{
    public class RunSpawnBehaviour : OneShotAction
    {
        protected override IOneShotAction BuildRuntimeImpl()
        {
            return new RunSpawnBehaviourRuntime();
        }
    }

    public class RunSpawnBehaviourRuntime : BaseOneShotActionRuntime<RunSpawnBehaviour>
    {
        private SpawnOnDeath _spawn;
        public override void OnAwake(StateMachineBehaviour machine)
        {
            base.OnAwake(machine);
            if (!machine.TryGetComponent(out _spawn))
            {
                Debug.LogError($"{machine.gameObject.name} has no {nameof(SpawnOnDeath)}");
            }
        }

        public override void Execute()
        {
            if (_spawn == null) return;
            
            _spawn.SpawnObjects();
        }
    }
}