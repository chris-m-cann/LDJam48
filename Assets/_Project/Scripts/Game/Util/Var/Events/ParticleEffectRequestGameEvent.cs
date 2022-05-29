//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using LDJam48;

namespace Util.Var.Events
{
    using UnityEngine;
    using Util.Var.Observe;
    
    [CreateAssetMenu(menuName = "Custom/Event/LDJam48.ParticleEffectRequest")]
    public sealed class ParticleEffectRequestGameEvent : GameEvent<LDJam48.ParticleEffectRequest>
    {
        public void Raise(Transform transform)
        {
            Value = new ParticleEffectRequest
            {
                Position = transform.position,
                Rotation = transform.rotation,
                Scale = transform.localScale
            };
            Raise();
        }
    }
    [System.Serializable()]
    public sealed class ParticleEffectRequestEventReference : EventReference<ParticleEffectRequestGameEvent, ObservableParticleEffectRequestVariable, LDJam48.ParticleEffectRequest>
    {
    }
}
