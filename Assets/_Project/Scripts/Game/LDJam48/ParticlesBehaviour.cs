using System;
using Sirenix.OdinInspector;
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
            [Tooltip("name just used for you to tell them apart")]
            public string Name;
            public bool IsLoopingEffect;
            [ShowIf("@!IsLoopingEffect")]
            public ParticleEffectRequestGameEvent Channel;
            [Tooltip("position to spawn the effect, will default to gameObject.transform if null")]
            [ShowIf("@!IsLoopingEffect")]
            public Transform Position;
            [ShowIf("@IsLoopingEffect")]
            public ParticleSystem LoopingParticles;
        }

        [ListDrawerSettings(ShowIndexLabels = true, Expanded = true)]
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
                Rotation = t.rotation,
                Scale = t.localScale
            });
        }

        private void PlayLoopingEffect(ParticleEffectData effect)
        {
            if (effect.LoopingParticles != null)
            {
                effect.LoopingParticles.gameObject.SetActive(true);
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
                effect.LoopingParticles.gameObject.SetActive(false);
            }
        }
    }
}