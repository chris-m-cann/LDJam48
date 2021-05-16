using System;
using Util.Var.Observe;

namespace Util.Var
{
    [Serializable]
    public class BoolReference : VariableReference<BoolVariable, ObservableBoolVariable, bool>
    {
    }
}