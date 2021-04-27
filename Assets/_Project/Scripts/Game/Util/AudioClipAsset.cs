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
}