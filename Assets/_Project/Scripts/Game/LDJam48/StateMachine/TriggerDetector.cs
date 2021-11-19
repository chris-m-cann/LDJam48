using System;
using UnityEngine;
using Util;

namespace LDJam48.StateMachine
{
    public class TriggerDetector : Detector
    {
        [SerializeField] private LayerMask mask;

        private void Update()
        {
            
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (enabled && mask.Contains(other.gameObject.layer))
            {
               InvokeOnDetected(other.gameObject);
            }
        }
    }
}