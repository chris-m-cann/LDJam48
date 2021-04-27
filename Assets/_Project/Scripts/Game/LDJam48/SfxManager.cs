using System;
using UnityEngine;
using Util;
using Util.Events;

namespace LDJam48
{
    public class SfxManager : MonoBehaviour
    {
        [SerializeField] private AudioClipAssetGameEvent channel;

        private AudioSource[] _sources;
        private int _idx;

        private void Awake()
        {
            _sources = GetComponentsInChildren<AudioSource>();
        }

        private void OnEnable()
        {
            channel.OnEventTrigger += PlayAsset;
        }


        private void OnDisable()
        {
            channel.OnEventTrigger -= PlayAsset;
        }

        private void PlayAsset(AudioClipAsset obj)
        {
            var clip = obj.Clips.RandomElement();

            var source = _sources[_idx];
            _idx = (_idx + 1) % _sources.Length;


            source.Stop();
            clip.SetSourceDetails(source);
            source.Play();
        }
    }
}