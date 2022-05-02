using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using Util.Var;
using Util.Var.Observe;

namespace LDJam48
{
    public class OnPauseResume : MonoBehaviour
    {
        [SerializeField] private ObservableStringVariable activeActionMap;
        [SerializeField] private string pauseActionMap = "Menu";
        [SerializeField] private BoolReference isPaused;

        [SerializeField] private UnityEvent onPause;
        [SerializeField] private UnityEvent onResume;


        private string _prevMap;

        public void OnPause()
        {
            _prevMap = activeActionMap.Value;
            activeActionMap.Value = pauseActionMap;
            isPaused.Value = true;
            onPause?.Invoke();
        }

        public void OnResume()
        {
            activeActionMap.Value = _prevMap;
            _prevMap = pauseActionMap;
            isPaused.Value = false;
            onResume?.Invoke();
        }

        public void TogglePause()
        {
            if (isPaused.Value)
            {
                OnResume();
            }
            else
            {
                OnPause();
            }
        }

        private void OnDestroy()
        {
            if (activeActionMap.Value == pauseActionMap)
            {
                activeActionMap.Value = _prevMap;
            }
        }
    }
}