using System;
using UnityEngine;
using UnityEngine.Events;

namespace Util
{
    public class OnKeyPressEvent : MonoBehaviour
    {
        [SerializeField] private KeyCode key = KeyCode.Escape;
        [SerializeField] private UnityEvent onKeyDown;


        private void Update()
        {
            if (Input.GetKeyDown(key))
            {
                onKeyDown.Invoke();
            }
        }
    }
}