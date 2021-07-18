using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using Util.Var.Events;
using Util.Var.Observe;

namespace LDJam48
{
    public class TriggerAction : MonoBehaviour
    {
        public bool isLeft = true;
        [SerializeField] private ObservableStringVariable actionMap;
        [SerializeField] private Vector2EventReference onDashingPress;
        [SerializeField] private VoidGameEvent onSlamPress;


        private InputControl _control;
        private Action _oneToTrigger;
        private Vector2 _direction;

        private void Start()
        {
            _control =  (isLeft) ? Keyboard.current.leftArrowKey : Keyboard.current.rightArrowKey;
            _direction = (isLeft) ? Vector2.left : Vector2.right;
            UpdateActionMap(actionMap.Value);
        }

        private void OnEnable()
        {
            actionMap.OnEventTrigger += UpdateActionMap;
        }


        private void OnDisable()
        {
            actionMap.OnEventTrigger -= UpdateActionMap;
        }

        void UpdateActionMap(string value)
        {
            if (value != "Dashing")
            {
                _oneToTrigger = () => onDashingPress.Raise(_direction);
            }

            else
            {
                _oneToTrigger = () => onSlamPress.Raise();
            }
        }

        public void OnClick()
        {
            _oneToTrigger.Invoke();
        }

        public void OnClickOld()
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