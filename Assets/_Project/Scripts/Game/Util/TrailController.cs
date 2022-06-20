using System;
using UnityEngine;

namespace Util
{
    [RequireComponent(typeof(TrailRenderer))]
    public class TrailController : MonoBehaviour
    {
        [SerializeField] private bool clearOnEnable;
        
        private TrailRenderer _trail;

        private void Awake()
        {
            _trail = GetComponent<TrailRenderer>();
        }

        private void OnEnable()
        {
            if (clearOnEnable)
            {
                _trail.Clear();
            }
        }
    }
}