using System;
using UnityEngine;

namespace LDJam48.StateMachine.Conditions
{
    [Serializable]
    public class WasDetectorTriggered : Condition
    {
        public int DetectorIndex = 0;
        protected override ICondition BuildRuntimeImpl()
        {
            return new WasDetectorTriggeredRuntime();
        }
    }
    
    public class WasDetectorTriggeredRuntime: BaseConditionRuntime<WasDetectorTriggered>
    {
        private Detector _detector;
        private bool _wasTriggered;
        public override void OnAwake(StateMachineBehaviour machine)
        {
            base.OnAwake(machine);
            var detectors = machine.GetComponent<Detectors>();
            if (detectors == null)
            {
                if (_source.DetectorIndex == 0)
                {
                    _detector = machine.GetComponent<Detector>();
                }
            }
            else
            {
                _detector = detectors.GetDetector(_source.DetectorIndex);
            }

            if (_detector == null)
            {
                Debug.LogError($"{_machine.name} : {Name} : No Detector found!");
            }
        }

        public override void OnStateEnter()
        {
            base.OnStateEnter();
            if (_detector != null)
            {
                _detector.OnDetected += OnDetected;
            }
        }

        public override void OnStateExit()
        {
            base.OnStateExit();
            if (_detector != null)
            {
                _detector.OnDetected -= OnDetected;
            }
        }

        private void OnDetected(GameObject o)
        {
            _wasTriggered = true;
        }

        public override bool Evaluate()
        {
            var tmp = _wasTriggered;
            _wasTriggered = false;
            return tmp;
        }
    }
}