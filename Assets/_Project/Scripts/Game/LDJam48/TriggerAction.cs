using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

namespace LDJam48
{
    public class TriggerAction : MonoBehaviour
    {
        public bool isLeft = true;

        private InputControl _control;

        private void Start()
        {
            _control =  (isLeft) ? Keyboard.current.leftArrowKey : Keyboard.current.rightArrowKey;
        }

        public void OnClick()
        {
            InputEventPtr eventPtr;
            using (StateEvent.From(_control.device, out eventPtr))
            {
                _control.WriteValueIntoEvent(1f, eventPtr);
                InputSystem.QueueEvent(eventPtr);
            }

            using (StateEvent.From(_control.device, out eventPtr))
            {
                _control.WriteValueIntoEvent(0f, eventPtr);
                InputSystem.QueueEvent(eventPtr);
            }
        }
    }
}