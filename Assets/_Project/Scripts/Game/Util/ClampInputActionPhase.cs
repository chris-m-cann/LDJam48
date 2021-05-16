using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Util
{
    public class ClampInputActionPhase: MonoBehaviour
    {
        [Serializable] public struct ActionEvent
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


        [SerializeField] private ActionEvent[] actions;


        private void OnEnable()
        {
            foreach (var action in actions)
            {
                action.Action.action.started += action.Cb;
                action.Action.action.performed += action.Cb;
                action.Action.action.canceled += action.Cb;
            }
        }

        private void OnDisable()
        {
            foreach (var action in actions)
            {
                action.Action.action.started -= action.Cb;
                action.Action.action.performed -= action.Cb;
                action.Action.action.canceled -= action.Cb;
            }
        }
    }
}