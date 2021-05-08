using UnityEngine;
using Util.Var.Observe;

namespace Util.Var.Events
{
    [CreateAssetMenu(menuName = "Custom/Event/Void")]
    public class VoidGameEvent : ObservableVariable<Void>
    {
        public new void Raise() => Raise(Void.Instance);
    }
}