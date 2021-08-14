using UnityEditor;
using UnityEngine;
using Util;
using Util.Var.Events;
using Util.Var.Observe;

namespace LDJam48.Stats
{
    [CreateAssetMenu(menuName = "Custom/Stat/CumulativeInt")]
    public class CumulativeIntStat : EventDrivenStat<IntEventReference, ObservableIntVariable, int>
    {
        protected override void OnUpdate(int value)
        {
            Current.Value += value;
        }
    }
}