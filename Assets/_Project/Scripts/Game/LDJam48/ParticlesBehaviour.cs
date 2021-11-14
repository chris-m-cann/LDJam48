using System;
using UnityEngine;
using Util;
using Util.Var.Events;

namespace LDJam48
{
    public class ParticlesBehaviour : MonoBehaviour
    {
        [Serializable]
        public struct ParticleEffectData
        {
            public bool IsLoopingEffect;
            [Header("One Shot Effect Parameters")]
            public ParticleEffectRequestGameEvent Channel;
            [Tooltip("position to spawn the effect, will default to gameObject.transform if null")]
            public Transform Position;
            [Header("Looping Effect Parameters")]
            public ParticleSystem LoopingParticles;
        }

        [SerializeField] private ParticleEffectData[] effects;

        public void PlayEffect(int idx)
        {
            if (!effects.HasIndex(idx))
            {
                Debug.LogError($"{name}.ParticlesBehaviour.PlayEffect does not support index {idx}");
                return;
            }

            var effect = effects[idx];

            if (effect.IsLoopingEffect)
            {
                PlayLoopingEffect(effect);
            }
            else
            {
                PlayOneShotEffect(effect);
            }
        }

        private void PlayOneShotEffect(ParticleEffectData effect)
        {
            var t = effect.Position == null ? transform : effect.Position;
            effect.Channel.Raise(new ParticleEffectRequest
            {
                Position = t.position,
                Rotation = Quaternion.identity,
                Scale = Vector3.one
            });
        }

        private void PlayLoopingEffect(ParticleEffectData effect)
        {
            if (effect.LoopingParticles != null)
            {
                effect.LoopingParticles.Play();
            }
        }

        public void StopEffect(int idx)
        {
            if (!effects.HasIndex(idx))
            {
                Debug.LogError($"{name}.ParticlesBehaviour.StopEffect does not support index {idx}");
                return;
            }
            
            var effect = effects[idx];

            if (effect.IsLoopingEffect && effect.LoopingParticles != null)
            {
                effect.LoopingParticles.Stop();
            }
        }
    }
}