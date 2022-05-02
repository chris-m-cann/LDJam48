using System;
using LDJam48.Save;
using UnityEngine;
using UnityEngine.Audio;

namespace LDJam48
{
    // todo (chris) seperate the savable settings values and setting the mixer itself
    [CreateAssetMenu(menuName = "Custom/Save/Saveable/SoundSettings")]
    public class SoundSettingsSaveableSO : SaveableSOT<SoundSettingsSaveable>
    {
        [SerializeField] private AudioMixer masterMixer;
        [SerializeField] private string masterVolumeParam;
        
        [SerializeField] private AudioMixer musicMixer;
        [SerializeField] private string musicVolumeParam;
        
        [SerializeField] private AudioMixer sfxMixer;
        [SerializeField] private string sfxVolumeParam;

        public override void LoadComplete()
        {
            base.LoadComplete();
            Debug.Log($"Setting levels, {Data.musicLevel}, {Data.sfxLevel}");
            SetMasterVolumeLevel(Data.masterVolume);
            SetMusicVolumeLevel(Data.musicLevel);
            SetSfxVolumeLevel(Data.sfxLevel);
        }

        public void SetMasterVolumeLevel(float level)
        {
            float volumeLevel = MapVolume(level);
            Debug.Log($"setting {masterVolumeParam} to {volumeLevel}");
            masterMixer.SetFloat(masterVolumeParam, volumeLevel);
            Data.masterVolume = level;
        }

        private float MapVolume(float normalisedLevel)
        {
            float max = 1;
            float min = .0001f;
            float level = Mathf.Lerp(min, max, normalisedLevel);
            return Mathf.Log10(level) * 20; // move it to the -80 -> 0 range that the volume actually moves in
        }

        public void SetMusicVolumeLevel(float level)
        {
            float volumeLevel = MapVolume(level);
            musicMixer.SetFloat(musicVolumeParam, volumeLevel);
            Data.musicLevel = level;
        }
        
        public void SetSfxVolumeLevel(float level)
        {
            float volumeLevel = MapVolume(level);
            sfxMixer.SetFloat(sfxVolumeParam, volumeLevel);
            Data.sfxLevel = level;
        }
    }

    [Serializable]
    public struct SoundSettingsSaveable : ISaveable
    {
        public float masterVolume;
        public float musicLevel;
        public float sfxLevel;
    }
}