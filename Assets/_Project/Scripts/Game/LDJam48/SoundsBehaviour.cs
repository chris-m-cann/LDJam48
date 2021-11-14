using UnityEngine;
using Util;
using Util.Var.Events;

namespace LDJam48
{
    public class SoundsBehaviour : MonoBehaviour
    {
        [SerializeField] private AudioClipAssetGameEvent channel;

        [SerializeField] private AudioClipAsset[] clips;


        public void PlaySound(int idx)
        {
            if (!clips.HasIndex(idx))
            {
                Debug.LogError($"{name}.SoundsBehaviour.PlaySound does not have a clip at index {idx}");
                return;
            }
            
            channel.Raise(clips[idx]);
        }
    }
}