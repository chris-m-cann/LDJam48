using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Util.UI
{
    public class UiNativation : MonoBehaviour
    {
        [SerializeField] private EventSystem eventSystem;
        [SerializeField] private GameObject firstSelected;
        [SerializeField] private float deadzone = .2f;


        private void Update()
        {
            if (eventSystem.currentSelectedGameObject != null) return;


            if (DirectionsTriggered())
            {
                eventSystem.SetSelectedGameObject(firstSelected);
            }
        }

        private bool DirectionsTriggered()
        {
            var keyboard = Keyboard.current;
            var gamepad = Gamepad.current;

            return keyboard.leftArrowKey.wasPressedThisFrame ||
                keyboard.rightArrowKey.wasPressedThisFrame ||
                keyboard.upArrowKey.wasPressedThisFrame ||
                keyboard.downArrowKey.wasPressedThisFrame ||
                gamepad.leftStick.ReadValue().magnitude > deadzone;
        }

        public void PrintMessage(string message) => Debug.Log(message);
    }
}
