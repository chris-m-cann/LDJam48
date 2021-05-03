using UnityEngine;
using Util.Variable;

namespace Util.Events
{
    [CreateAssetMenu(menuName = "Custom/Events/Void")]
    public class VoidGameEvent : ObservableVariable<Void>
    {
        public new void Raise() => Raise(Void.Instance);
    }
}