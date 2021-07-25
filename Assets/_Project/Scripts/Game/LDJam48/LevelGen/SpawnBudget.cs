using System;
using UnityEngine;
using Util.Var;

namespace LDJam48.LevelGen
{
    [CreateAssetMenu(menuName = "Custom/Level/SpawnBudget")]
    public class SpawnBudget: ScriptableObject
    {   
        [SerializeField] private float start;
        [SerializeField] private float max;
        [SerializeField] private float ratePerMeter;
        [SerializeField] private IntReference distanceTravelled;
        
        [NonSerialized] private float _current;
        [NonSerialized] private int _travelled;

        private void OnEnable()
        {
            _current = start;
            _travelled = 0;
        }

        public bool AllocateBudget(float cost)
        {
            var travelledSinceLastAllocate = distanceTravelled.Value - _travelled;
            _travelled = distanceTravelled.Value;
            
            _current = Mathf.Min(_current + ratePerMeter * travelledSinceLastAllocate, max);

            if (_current < cost) return false;

            _current -= cost;
            return true;
        }
    }
}