using UnityEngine;
using Util;
using Util.UI;
using Util.Var.Observe;

namespace LDJam48
{
    [CreateAssetMenu(menuName = "Custom/Model/Run")]
    public class RunModel : Model
    {
        [Nested] public ObservableIntVariable DistanceTravelled;
        [Nested] public ObservableIntVariable TotalGemsCollected;
        [Nested] public ObservableIntVariable CurrentGems;
        // add in time began? time ended?
    }
}