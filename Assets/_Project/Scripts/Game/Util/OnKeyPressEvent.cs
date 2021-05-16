using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace Util
{
    public class OnKeyPressEvent : MonoBehaviour
    {
        [SerializeField] private InputAction action;
        [SerializeField] private UnityEvent onKeyDown;


        private void OnEnable()
        {
            action.performed += Invoke;
        }


        private void OnDisable()
        {
            action.performed -= Invoke;
        }

        private void Invoke(InputAction.CallbackContext ctx) => onKeyDown.Invoke();
    }
}