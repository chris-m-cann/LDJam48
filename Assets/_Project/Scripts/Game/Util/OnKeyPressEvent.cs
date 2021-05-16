using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace Util
{
    public class OnKeyPressEvent : MonoBehaviour
    {
        [SerializeField] private InputActionReference[] actions;
        [SerializeField] private UnityEvent onKeyDown;


        private void OnEnable()
        {
            foreach (var action in actions)
            {
                action.action.performed += Invoke;
            }

        }


        private void OnDisable()
        {
            foreach (var action in actions)
            {
                action.action.performed -= Invoke;
            }
        }

        private void Invoke(InputAction.CallbackContext ctx) => onKeyDown.Invoke();
    }
}