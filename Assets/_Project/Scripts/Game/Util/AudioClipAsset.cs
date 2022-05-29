using System;
using System.Diagnostics.Tracing;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Audio;
using Object = UnityEngine.Object;

namespace Util
{
    [CreateAssetMenu(menuName = "Custom/AudioClip")]
    public class AudioClipAsset : ScriptableObject
    {
        public AudioClipEx[] Clips;
    }

    [Serializable]
    public class AudioClipEx
    {
        public bool Enabled = true;
        public AudioClip clip;
        [Range(0, 1)] public float volume = 1f;
        public AudioMixerGroup mixer;
        public bool Loop;
        public bool VaryPitch;
        
        [ShowIf("@!VaryPitch"), Range(0, 3)]
        public float Pitch = 1;
        [ShowIf("@VaryPitch"), RangeSlider(-3, 3)]
        public Range PitchFactor;
        public AudioTransition Transition = new AudioTransition
        {
            TransitionType = AudioTransition.Type.PopIn
        };


        public void SetSourceDetails(AudioSource source)
        {
            source.clip = clip ?? source.clip;
            source.volume = volume;
            source.outputAudioMixerGroup = mixer ?? source.outputAudioMixerGroup;
            source.loop = Loop;
            if (VaryPitch)
            {
                var factor = RandomEx.Range(PitchFactor);
                source.pitch = factor;
            }
            else
            {
                source.pitch = Pitch;
            }
        }

        [Button]
        public void PlaySound()
        {
            AudioSource source = Object.FindObjectOfType<AudioSource>();
            SetSourceDetails(source);
            source.Play();
        }
    }

    [Serializable]
    public struct AudioTransition
    {
        public enum Type
        {
            PopIn,
            FadeIn,
            CrossFade
        }

        public Type TransitionType;
        public float DurationIn;
        public float DurationOut;
    }
}