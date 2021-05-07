using System;
using UnityEngine;
using Util.Var.Observe;

namespace Util.Var
{
    [Serializable]
    public class FloatReference : VariableReference<FloatVariable, ObservableFloatVariable, float>
    {
    }
}