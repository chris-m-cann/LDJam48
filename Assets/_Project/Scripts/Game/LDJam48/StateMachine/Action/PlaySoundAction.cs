using System;
using UnityEngine;

namespace LDJam48.StateMachine.Action
{
    [Serializable]
    public class PlaySoundAction : OneShotAction
    {
        [SerializeField] private int soundIdx;

        public int SoundIdx => soundIdx;
        protected override IOneShotAction BuildRuntimeImpl()
        {
            return new PlaySoundActionRuntime();
        }
    }
    
    public class PlaySoundActionRuntime : BaseOneShotActionRuntime<PlaySoundAction>
    {
        private SoundsBehaviour _sounds;
        public override void OnAwake(StateMachineBehaviour machine)
        {
            base.OnAwake(machine);
            _sounds = machine.GetComponent<SoundsBehaviour>();
        }

        public override void Execute()
        {
            _sounds.PlaySound(_source.SoundIdx);
        }
    }
    
    [Serializable]
    public class PlayAudioSourceAction : OneShotAction
    {
        [SerializeField] private int sourceIndex;

        public int SourceIndex => sourceIndex;
        protected override IOneShotAction BuildRuntimeImpl()
        {
            return new PlayAudioSourceActionRuntime();
        }
    }
    
    public class PlayAudioSourceActionRuntime : BaseOneShotActionRuntime<PlayAudioSourceAction>
    {
        private SoundsBehaviour _sounds;
        public override void OnAwake(StateMachineBehaviour machine)
        {
            base.OnAwake(machine);
            _sounds = machine.GetComponent<SoundsBehaviour>();
        }

        public override void Execute()
        {
            _sounds.PlaySource(_source.SourceIndex);
        }
    }
    
    [Serializable]
    public class StopAudioSourceAction : OneShotAction
    {
        [SerializeField] private int sourceIndex;

        public int SourceIndex => sourceIndex;
        protected override IOneShotAction BuildRuntimeImpl()
        {
            return new StopAudioSourceActionRuntime();
        }
    }
    
    public class StopAudioSourceActionRuntime : BaseOneShotActionRuntime<StopAudioSourceAction>
    {
        private SoundsBehaviour _sounds;
        public override void OnAwake(StateMachineBehaviour machine)
        {
            base.OnAwake(machine);
            _sounds = machine.GetComponent<SoundsBehaviour>();
        }

        public override void Execute()
        {
            _sounds.StopSource(_source.SourceIndex);
        }
    }
}