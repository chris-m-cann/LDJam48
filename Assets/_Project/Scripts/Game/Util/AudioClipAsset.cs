using System;
using UnityEngine;
using UnityEngine.Audio;

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
        public AudioClip clip;
        [Range(0, 1)] public float volume = 1f;
        public AudioMixerGroup mixer;
        public bool Loop;
        public bool VaryPitch;
        [RangeSlider(-3, 3)]
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
                source.pitch = 1;
            }
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