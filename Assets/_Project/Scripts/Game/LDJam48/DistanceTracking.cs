using System;
using TMPro;
using UnityEngine;
using Util.Var;
using Util.Var.Events;
using Util.Var.Observe;

namespace LDJam48
{
    public class DistanceTracking : MonoBehaviour
    {
        [SerializeField] private ObservableIntVariable distanceVariable;
        [SerializeField] private GameObjectVariable target;
        [SerializeField] private VoidGameEvent onRunStart;
        


        private bool _started = false;
        private float _startDistance = -13;

        private void OnEnable()
        {
            distanceVariable.Value = 0;
            onRunStart.OnEventTrigger += StartRun;
            _started = false;
        }

        private void OnDisable()
        {
            onRunStart.OnEventTrigger -= StartRun;
            _started = false;
        }

        private void StartRun(Util.Void v)
        {
            _startDistance = GetTargetPosition();
            _started = true;
        }

        private float GetTargetPosition()
        {
            return target.Value.transform.position.y;
        }

        private void Update()
        {
            if (!_started) return;
            if (target.Value == null) return;

            var dist = (int)Mathf.Max(0, _startDistance - GetTargetPosition());

            distanceVariable.Value = dist;
        }
    }
}