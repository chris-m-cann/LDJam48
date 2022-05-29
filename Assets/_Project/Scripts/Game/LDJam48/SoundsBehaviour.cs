using UnityEngine;
using Util;
using Util.Var.Events;

namespace LDJam48
{
    public class SoundsBehaviour : MonoBehaviour
    {
        [SerializeField] private AudioClipAssetGameEvent channel;

        [SerializeField] private AudioClipAsset[] clips;
        [SerializeField] private AudioSource[] sources;


        public void PlaySound(int idx)
        {
            if (!clips.HasIndex(idx))
            {
                Debug.LogError($"{name}.SoundsBehaviour.PlaySound does not have a clip at index {idx}");
                return;
            }
            
            channel.Raise(clips[idx]);
        }

        public void PlaySource(int idx)
        {
            if (!sources.HasIndex(idx))
            {
                Debug.LogError($"{name}.SoundsBehaviour.PlaySource does not have a source at index {idx}");
                return;
            }
            
            sources[idx].Play();
        }
        
        public void StopSource(int idx)
        {
            if (!sources.HasIndex(idx))
            {
                Debug.LogError($"{name}.SoundsBehaviour.StopSource does not have a source at index {idx}");
                return;
            }
            
            sources[idx].Stop();
        }
    }
}