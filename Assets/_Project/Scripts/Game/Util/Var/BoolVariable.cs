using System;
using UnityEngine;
using Util.Var.Observe;

namespace Util.Var
{
    [CreateAssetMenu(menuName = "Custom/Variable/bool")]
    public class BoolVariable : Variable<bool>
    {

    }

    [Serializable]
    public class BoolReference : VariableReference<BoolVariable, ObservableBoolVariable, bool>
    {
    }
}