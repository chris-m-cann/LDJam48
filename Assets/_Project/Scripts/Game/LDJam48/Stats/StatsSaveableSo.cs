using System;
using System.Linq;
using LDJam48.Save;
using UnityEngine;
using Util;

namespace LDJam48.Stats
{
    
    [CreateAssetMenu(menuName = "Custom/Save/Saveable/Stats")]
    public class StatsSaveableSo : SaveableSO
    {
        [SerializeField] private StatT<int>[] intStats;


        public override ISaveable GetSaveable()
        {
            var data = new StatsSaveable
            {
                Ints = new Pair<string, int>[intStats.Length]
            };

            for (int i = 0; i < intStats.Length; i++)
            {
                data.Ints[i] = new Pair<string, int>(intStats[i].name, intStats[i].Value);
            }

            return data;
        }

        public override Type GetSaveableType()
        {
            return typeof(StatsSaveable);
        }

        public override void SetSaveable(ISaveable other)
        {
            if (other is StatsSaveable stats)
            {
                foreach (var stat in intStats)
                {
                    var value = FindValue(stats, stat.name);
                    if (value.HasValue)
                    {
                        stat.Value = value.Value;
                    }
                }
            }
        }

        private int? FindValue(StatsSaveable saveable, string key)
        {
            foreach (var pair in saveable.Ints)
            {
                if (pair.First == key) return pair.Second;
            }

            return null;
        }
    }

    [Serializable]
    public struct StatsSaveable : ISaveable
    {
        public Pair<string, int>[] Ints;
    }
}