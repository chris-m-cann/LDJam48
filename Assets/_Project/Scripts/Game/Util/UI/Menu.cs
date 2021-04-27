using System;
using UnityEngine;
using UnityEngine.Events;

namespace Util.UI
{
    public class Menu : MonoBehaviour
    {
        [SerializeField] private UnityEvent onMenuEnabled;
        [SerializeField] private UnityEvent onMenuDisabled;
        [SerializeField] private UnityEvent onSwitchedTo;
        [SerializeField] private UnityEvent onSwitchedFrom;

        private Action _onMenuEnabledComplete;
        private Action _onMenuDisabledComplete;
        private Action _onSwitchedToComplete;
        private Action _onSwitchedFromComplete;

        public void OnMenuEnabled(Action onComplete)
        {
            OnCall(onMenuEnabled, onComplete, ref _onMenuEnabledComplete);
        }

        public void CompleteOnMenuEnabled()
        {
            _onMenuEnabledComplete?.Invoke();
            ResetActions();
        }

        public void OnMenuDisabled(Action onComplete)
        {
            OnCall(onMenuDisabled, onComplete, ref _onMenuDisabledComplete);
        }

        public void CompleteOnMenuDisabled()
        {
            _onMenuDisabledComplete?.Invoke();
            ResetActions();
        }

        public void OnSwitchedTo(Action onComplete)
        {
            OnCall(onSwitchedTo, onComplete, ref _onSwitchedToComplete);
        }

        public void CompleteOnSwitchedTo()
        {
            _onSwitchedToComplete?.Invoke();
            ResetActions();
        }

        public void OnSwitchedFrom(Action onComplete)
        {
            OnCall(onSwitchedFrom, onComplete, ref _onSwitchedFromComplete);
        }

        public void CompleteOnSwitchedFrom()
        {
            _onSwitchedFromComplete?.Invoke();
            ResetActions();
        }

        private void OnCall(UnityEvent @event, Action onComplete, ref Action cache)
        {
            ResetActions();

            if (onComplete == null) return;

            if (@event.GetPersistentEventCount() == 0)
            {
                onComplete();
                return;
            }

            cache = onComplete;
            @event.Invoke();
        }

        private void ResetActions()
        {
            _onMenuEnabledComplete = null;
            _onMenuDisabledComplete = null;
            _onSwitchedToComplete = null;
            _onSwitchedFromComplete = null;
        }

    }
}
