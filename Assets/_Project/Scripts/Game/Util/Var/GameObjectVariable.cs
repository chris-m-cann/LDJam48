using UnityEngine;
using Util.Var.Observe;

namespace Util.Var
{
    [CreateAssetMenu(menuName = "Custom/Variable/GameObject")]
    public class GameObjectVariable : Variable<GameObject>
    {
    }

    [System.Serializable()]
    public sealed class GameObjectReference : VariableReference<GameObjectVariable, ObservableGameObjectVariable, GameObject>
    {
    }
}