using System;
using Unity.VisualScripting;
using UnityEngine;
using Util.Var;
using Util.Var.Observe;

namespace LDJam48
{
    public class PauseTimeStop : MonoBehaviour
    {
        [SerializeField] private ObservableBoolVariable isPaused;


        private float _oldTime = 1f;

        private void OnEnable()
        {
            if (isPaused != null)
            {
                isPaused.OnValueChanged += SetTime;

                isPaused.Reset();
            }

        }

        private void OnDisable()
        {
            if (isPaused != null)
            {
                isPaused.OnValueChanged -= SetTime;
            }

            Time.timeScale = 1f;
        }

        private void SetTime(bool paused)
        {
            if (paused)
            {
                _oldTime = Time.timeScale;
                Time.timeScale = 0f;
            }
            else
            {
                Time.timeScale = _oldTime;
            }
        }
    }
}