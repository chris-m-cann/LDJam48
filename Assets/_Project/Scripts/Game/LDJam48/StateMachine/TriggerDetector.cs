using System;
using UnityEngine;
using Util;

namespace LDJam48.StateMachine
{
    public class TriggerDetector : Detector
    {
        [SerializeField] private LayerMask mask;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (enabled && mask.Contains(other.gameObject.layer))
            {
                WasDetected = true;
            }
        }
    }
}