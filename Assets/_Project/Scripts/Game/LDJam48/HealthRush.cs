using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Util.Var.Events;
using Void = Util.Void;

namespace LDJam48
{
    public class HealthRush : MonoBehaviour
    {
        [SerializeField] private VoidGameEvent runStartedEvent;
        [SerializeField] private IntEventReference onHealthIncreaseEvent;
        [SerializeField] private float distanceBeforeActivation;
        [SerializeField] private float healthRegainPerUnitTravelled;
        [SerializeField] private UnityEvent onActivate;
        [SerializeField] private UnityEvent onDeactivate;


        private bool _runStarted;
        private float _lastResetYPos;
        private bool _activated;

        private void OnEnable()
        {
            runStartedEvent.OnEventTrigger += OnRunStarted;
        }

        private void OnDisable()
        {
            runStartedEvent.OnEventTrigger -= OnRunStarted;
        }

        private void OnRunStarted(Void v)
        {
            _lastResetYPos = transform.position.y;
            _runStarted = true;
        }

        private void Update()
        {
            if (!_runStarted || _activated) return;

            if (Mathf.Abs(transform.position.y - _lastResetYPos) >= distanceBeforeActivation)
            {
                Activate();
            }
        }

        private void Activate()
        {
            if (_activated) return;
            _activated = true;
            StartCoroutine(CoRegainHealth());
            onActivate?.Invoke();
        }

        private IEnumerator CoRegainHealth()
        {
            float positionLastFrame = transform.position.y;
            float unbankedHealthRegain = 0;

            while (true)
            {
                yield return null;

                float diff = Mathf.Abs(transform.position.y - positionLastFrame);
                positionLastFrame = transform.position.y;

                unbankedHealthRegain += diff * healthRegainPerUnitTravelled;

                if (unbankedHealthRegain > 1f)
                {
                    int gain = Mathf.FloorToInt(unbankedHealthRegain);
                    unbankedHealthRegain -= gain;
                    onHealthIncreaseEvent.Raise(gain);
                }
            }
        }

        // todo (chris) get an event on idle and call this ourselves
        public void Reset()
        {
            _lastResetYPos = transform.position.y;
            if (!_activated) return;
            StopAllCoroutines();
            _activated = false;
            onDeactivate?.Invoke();
        }
    }
}