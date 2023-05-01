using System;
using UnityEngine;
using UnityEngine.Events;

namespace Util.Var.Observe
{
    public abstract class ObservableTrackingBehaviour<T, TObservable> : MonoBehaviour
        where TObservable : ObservableVariable<T> where T : IComparable
    {
        [SerializeField] private TObservable observable;

        [SerializeField] private UnityEvent<T> onValueChanged;
        [SerializeField] private UnityEvent<T> onValueIncreased;
        [SerializeField] private UnityEvent<T> onValueDecreased;

        private T _prev;
        private bool _started;
        private bool _subscribed;

        private void Start()
        {
            _started = true;
            Subscribe();
        }

        private void OnEnable()
        {
            if (!_subscribed)
            {
                Subscribe();
            }
        }

        private void Subscribe()
        {
            if (observable == null) return;
            _prev = observable.Value;
            observable.OnValueChanged += OnEventRaised;
            _subscribed = true;
        }

        private void OnDisable()
        {
            if (observable == null) return;
            observable.OnValueChanged -= OnEventRaised;
            _subscribed = false;
        }

        public void OnEventRaised(T t)
        {
            if (!_started) return;
            
            onValueChanged?.Invoke(t);

            int compareResult = t.CompareTo(_prev);
            if (compareResult > 0)
            {
                onValueIncreased?.Invoke(t);
            }
            else if (compareResult < 0)
            {
                onValueDecreased?.Invoke(t);
            }

            _prev = t;
        }

        public void Reset()
        {
            _prev = observable.Value;
        }
    }
}