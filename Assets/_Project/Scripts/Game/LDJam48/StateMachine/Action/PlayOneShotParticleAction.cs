using System;
using UnityEngine;

namespace LDJam48.StateMachine.Action
{
    [Serializable]
    public class PlayOneShotParticleAction : OneShotAction
    {
        [SerializeField] private int effectIdx;

        public int EffectIdx => effectIdx;
        protected override IOneShotAction BuildRuntimeImpl()
        {
            return new PlayOneShotParticleActionRuntime();
        }
    }
    
    public class PlayOneShotParticleActionRuntime : BaseOneShotActionRuntime<PlayOneShotParticleAction>
    {
        private ParticlesBehaviour _particles;
        public override void OnAwake(StateMachineBehaviour machine)
        {
            base.OnAwake(machine);

            _particles = machine.GetComponent<ParticlesBehaviour>();
        }

        public override void Execute()
        {
            _particles.PlayEffect(_source.EffectIdx);
        }
    }
}