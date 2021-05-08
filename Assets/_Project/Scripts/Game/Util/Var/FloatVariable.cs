using System;
using UnityEngine;
using Util.Var.Observe;

namespace Util.Var
{
    [CreateAssetMenu(menuName = "Custom/Variable/float")]
    public class FloatVariable : Variable<float>
    {
    }

    [Serializable]
    public class FloatReference : VariableReference<FloatVariable, ObservableFloatVariable, float>
    {
    }
}