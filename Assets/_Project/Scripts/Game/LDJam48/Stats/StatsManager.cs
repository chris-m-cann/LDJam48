using System;
using UnityEngine;

namespace LDJam48.Stats
{
    public class StatsManager : MonoBehaviour
    {
        [SerializeField] private Stat[] stats;

        private void OnEnable()
        {
            foreach (var stat in stats)
            {
                stat.Register();
            }
        }

        private void OnDisable()
        {
            foreach (var stat in stats)
            {
                stat.Unregister();
            }
        }

        [ContextMenu("ResetStats")]
        public void ResetAllStats()
        {
            foreach (var stat in stats)
            {
                stat.Reset();
            }
        }
    }
}