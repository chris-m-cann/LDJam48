using System;
using UnityEngine;

namespace LDJam48.StateMachine.Action
{
    [Serializable]
    public class EnableDetectorStateAction : StateAction
    {
        public int DetectorIdx;

        protected override IStateAction BuildRuntimeImpl()
        {
            return new EnableDetectorStateActionRuntime();
        }
    }

    public class EnableDetectorStateActionRuntime : BaseStateActionRuntime<EnableDetectorStateAction>
    {
        private Detectors _detectors;

        public override void OnAwake(StateMachineBehaviour machine)
        {
            base.OnAwake(machine);
            _detectors = machine.GetComponent<Detectors>();

            if (_detectors == null)
            {
                Debug.LogError($"{_machine.name} : {Name} : detectors not found!");
            }
        }

        public override void OnStateEnter()
        {
            base.OnStateEnter();
            if (_detectors != null)
            {
                _detectors.EnableDetector(_source.DetectorIdx);
            }
        }

        public override void OnStateExit()
        {
            base.OnStateExit();
            if (_detectors != null)
            {
                _detectors.DisableDetector(_source.DetectorIdx);
            }
        }
    }
}