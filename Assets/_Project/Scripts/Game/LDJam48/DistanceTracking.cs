using System;
using TMPro;
using UnityEngine;
using Util.Variable;

namespace LDJam48
{
    public class DistanceTracking : MonoBehaviour
    {
        [SerializeField] private ObservableIntVariable distanceVariable;
        [SerializeField] private float startDistance = -13;

        private Transform _player;

        private void Start()
        {
            _player = FindObjectOfType<PlayerController>()?.transform;
        }

        private void Update()
        {
            if (_player == null) return;
            if (distanceVariable == null) return;

            var dist = (int)Mathf.Max(0, (startDistance - _player.position.y));

            distanceVariable.Value = dist;
        }
    }
}