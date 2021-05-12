using System;
using UnityEngine;

namespace LDJam48
{
    public class InputManager: MonoBehaviour
    {
        public event Action OnDashLeft;
        public event Action OnDashRight;
        public event Action OnSlash;
        public event Action OnSlam;

        public bool OnDashLeftPressed = false;
        public bool OnDashRightPressed = false;
        public bool OnSlashPressed = false;
        public bool OnSlamPressed = false;

        public void InvokeDashLeft() => OnDashLeft?.Invoke();
        public void InvokeDashRight() => OnDashRight?.Invoke();
        public void InvokeSlash() => OnSlash?.Invoke();
        public void InvokeSlam() => OnSlam?.Invoke();


        private void Update()
        {
            OnDashLeftPressed = false;
            OnDashRightPressed = false;
            OnSlashPressed = false;
            OnSlamPressed = false;

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                OnSlashPressed = true;
                InvokeSlash();
            }


            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                OnDashLeftPressed = true;
                InvokeDashLeft();
            }


            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                OnDashRightPressed = true;
                InvokeDashRight();
            }


            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                OnSlamPressed = true;
                InvokeSlam();
            }
        }
    }
}