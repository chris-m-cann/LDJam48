using System;
using UnityEngine;

namespace LDJam48.StateMachine.Conditions
{
    [CreateAssetMenu(menuName = "Custom/StateMachine/Condition/DetectorTriggered")]
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
        private Func<bool> _wasTriggered;
        public override void OnAwake(StateMachineBehaviour machine)
        {
            base.OnAwake(machine);
            var detectors = machine.GetComponent<Detectors>();
            Detector detector = null;
            if (detectors == null)
            {
                if (_source.DetectorIndex == 0)
                {
                    detector = machine.GetComponent<Detector>();
                }
            }
            else
            {
                detector = detectors.GetDetector(_source.DetectorIndex);
            }

            if (detector != null)
            {
                _wasTriggered = () => detector.WasDetected;
            }
            else
            {
                Debug.LogError($"{_machine.name} : {Name} : No Detector found!");
                _wasTriggered = () => false;
            }
        }

        public override bool Evaluate()
        {
            return _wasTriggered.Invoke();
        }
    }
}