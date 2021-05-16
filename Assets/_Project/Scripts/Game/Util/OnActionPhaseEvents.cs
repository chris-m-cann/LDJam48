using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Util
{
    public class OnActionPhaseEvents: MonoBehaviour
    {
        [Serializable] public struct ButtonActionEvent
        {
            public InputActionPhase Phase;
            public InputActionReference Action;
            public UnityEvent FollowOn;

            public void Cb(InputAction.CallbackContext ctx)
            {
                if (ctx.phase == Phase)
                {
                    FollowOn.Invoke();
                }
            }
        }

        [Serializable] public struct Vector2ActionEvent
        {
            public InputActionPhase Phase;
            public InputActionReference Action;
            public UnityEvent<Vector2> FollowOn;

            public void Cb(InputAction.CallbackContext ctx)
            {
                if (ctx.phase == Phase)
                {
                    FollowOn.Invoke(ctx.ReadValue<Vector2>());
                }
            }
        }


        [SerializeField] private ButtonActionEvent[] buttonActions;
        [SerializeField] private Vector2ActionEvent[] vec2Actions;


        private void OnEnable()
        {
            foreach (var action in buttonActions)
            {
                action.Action.action.started += action.Cb;
                action.Action.action.performed += action.Cb;
                action.Action.action.canceled += action.Cb;
            }

            foreach (var action in vec2Actions)
            {
                action.Action.action.started += action.Cb;
                action.Action.action.performed += action.Cb;
                action.Action.action.canceled += action.Cb;
            }
        }

        private void OnDisable()
        {
            foreach (var action in buttonActions)
            {
                action.Action.action.started -= action.Cb;
                action.Action.action.performed -= action.Cb;
                action.Action.action.canceled -= action.Cb;
            }

            foreach (var action in vec2Actions)
            {
                action.Action.action.started -= action.Cb;
                action.Action.action.performed -= action.Cb;
                action.Action.action.canceled -= action.Cb;
            }
        }
    }
}