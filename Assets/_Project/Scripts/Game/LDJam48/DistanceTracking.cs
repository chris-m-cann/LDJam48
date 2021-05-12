using System;
using TMPro;
using UnityEngine;
using Util.Var;
using Util.Var.Observe;

namespace LDJam48
{
    public class DistanceTracking : MonoBehaviour
    {
        [SerializeField] private ObservableIntVariable distanceVariable;
        [SerializeField] private float startDistance = -13;
        [SerializeField] private GameObjectVariable target;


        private void Update()
        {
            if (target.Value == null) return;

            var dist = (int)Mathf.Max(0, (startDistance - target.Value.transform.position.y));

            distanceVariable.Value = dist;
        }
    }
}