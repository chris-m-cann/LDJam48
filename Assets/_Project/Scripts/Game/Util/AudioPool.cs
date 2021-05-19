using DG.Tweening;
using UnityEngine;
using Util.Var.Events;

namespace Util
{
    public class AudioPool : MonoBehaviour
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


            switch (clip.Transition.TransitionType)
            {
                case AudioTransition.Type.FadeIn:
                    FadeInSound(source, clip);
                    break;
                case AudioTransition.Type.CrossFade:
                    CorssfadeToSound(source, clip);
                    break;
                case AudioTransition.Type.PopIn: // fall through
                default:
                    PopInSound(source, clip);
                    break;
            }
        }

        public void PopInSound(AudioSource source, AudioClipEx clip)
        {
            source.Stop();
            clip.SetSourceDetails(source);
            source.Play();
        }

        public void FadeInSound(AudioSource source, AudioClipEx clip)
        {
            clip.SetSourceDetails(source);
            source.volume = 0f;
            source.Play();

            source.DOFade(clip.volume, clip.Transition.DurationIn).Play();
        }

        public void CorssfadeToSound(AudioSource source, AudioClipEx clip)
        {
            Debug.Log($"Crosfading to sourd {clip.clip.name}");

            DOTween.Sequence()
                .Append(source.DOFade(0, clip.Transition.DurationOut))
                .AppendCallback(() =>
                {
                    source.Stop();
                    clip.SetSourceDetails(source);
                    source.volume = 0;
                    source.Play();
                })
                .Append(source.DOFade(clip.volume, clip.Transition.DurationIn))
                .Play();
        }
    }
}