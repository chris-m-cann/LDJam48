using System;
using UnityEngine;

namespace LDJam48.StateMachine.Action
{
    [Serializable]
    public class LoopingParticlesAction : StateAction
    {
        [SerializeField] private int effectIdx;

        public int EffectIdx => effectIdx;
        protected override IStateAction BuildRuntimeImpl()
        {
            return new LoopingParticlesActionRuntime();
        }
    }
    
    public class LoopingParticlesActionRuntime : BaseStateActionRuntime<LoopingParticlesAction>
    {
        private ParticlesBehaviour _particles;
        public override void OnAwake(StateMachineBehaviour machine)
        {
            base.OnAwake(machine);

            _particles = machine.GetComponent<ParticlesBehaviour>();
        }

        public override void OnStateEnter()
        {
            base.OnStateEnter();
            _particles.PlayEffect(_source.EffectIdx);
        }

        public override void OnStateExit()
        {
            base.OnStateExit();
            _particles.StopEffect(_source.EffectIdx);
        }
    }
}