using System;
using UnityEngine;
using Util.Var.Observe;

namespace Util.Var.Events
{
    [CreateAssetMenu(menuName = "Custom/Events/int")]

    public class IntGameEvent : GameEvent<int>
    {
    }

    [Serializable]
    public class IntGameEventReference : EventReference<IntGameEvent, ObservableIntVariable, int>
    {

    }
}