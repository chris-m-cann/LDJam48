using System;
using Unity.VisualScripting;
using UnityEngine;
using Util.Events;
using Util.Variable;

namespace LDJam48
{
    public class GemTracking: MonoBehaviour
    {
        [SerializeField] private ObservableIntVariable displayGems;
        [SerializeField] private ObservableIntVariable totalGems;
        [SerializeField] private ObservableIntVariable health;
        [SerializeField] private int gemsTo1Life = 100;
        [SerializeField] private ObservableIntVariable onGemPickup;

        private void OnEnable()
        {
            onGemPickup.OnEventTrigger += AddGems;
            displayGems.Reset();
        }

        private void OnDisable()
        {
            onGemPickup.OnEventTrigger -= AddGems;
        }

        public void AddGems(int value)
        {
            var before = totalGems.Value / gemsTo1Life;
            totalGems.Value += value;
            var after = totalGems.Value / gemsTo1Life;
            var diff = after - before;

            if (diff > 0)
            {
                health.Value += diff;
            }

            displayGems.Value = totalGems.Value % gemsTo1Life;
        }
    }
}