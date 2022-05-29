using System;
using System.Collections;
using DG.Tweening;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Util;
using Util.Var;
using Util.Var.Observe;

namespace LDJam48
{
    [RequireComponent(typeof(Image))]
    public class DeathCardHealthUI : MonoBehaviour
    {
        [SerializeField] private ObservableIntVariable totalHealth;
        [SerializeField] private IntReference healthPerLife;
        [SerializeField] private float tweenDelay;
        [SerializeField] private float minTweenSpeed;
        [SerializeField] private float maxTweenTime;

        [SerializeField] private AudioSource source;
        [SerializeField] private AudioClipAsset clip;
        [SerializeField] private float minPitch;
        [SerializeField] private float maxPitch;
        [SerializeField] private float gap;
        
        
        [SerializeField] private UnityEvent<int> onHealthIncrease;
        [SerializeField] private UnityEvent<int> onLivesIncrease;
        

        private Image _image;
        private Material _mat;
        
        private static readonly int Fill = Shader.PropertyToID("Fill");
        private Tweener _tweener;
        private int _lastLives = 0;

        private void Awake()
        {
            _image = GetComponent<Image>();
            _mat = _image.material;
            UpdateTotalHealth(0);
        }

        private void OnEnable()
        {
            this.ExecuteAfterUnscaled(tweenDelay, TweenToCurrent);
        }

        private void TweenToCurrent()
        {
            var time = Mathf.Min(maxTweenTime, totalHealth.Value * minTweenSpeed);
            _tweener = DOTween
                .To(UpdateTotalHealth, startValue: 0, endValue: totalHealth.Value, time)
                .SetUpdate(true);

            _tweener.Play();

            StartCoroutine(CoTweenSoundPitch());
        }

        private IEnumerator CoTweenSoundPitch()
        {
            
            var time = Mathf.Min(maxTweenTime, totalHealth.Value * minTweenSpeed);
            var start = Time.unscaledTime;
            var end = start + time;
            var elem = clip.Clips.RandomElement();
            float length = elem.clip.length;
            float lastPlaying = 0;
            
            while (Time.unscaledTime < end)
            {
                var t = (Time.unscaledTime - start) / time;
                var pitch = Mathf.Lerp(minPitch, maxPitch, t);

                elem.SetSourceDetails(source);
                source.pitch = pitch;
                source.Play();

                yield return new WaitForSecondsRealtime(gap);
            }

        }

        private void OnDisable()
        {
            if (_tweener != null)
            {
                _tweener.Kill();
            }
        }

        private void UpdateTotalHealth(float health)
        {
            var lives = Mathf.FloorToInt(health / healthPerLife.Value);
            if (lives != _lastLives)
            {
                onLivesIncrease?.Invoke(lives);
            }
            
            _lastLives = lives;
            
            var healthThisLife = health - (lives * healthPerLife.Value);
            var percentFill = healthThisLife / healthPerLife.Value;
            _mat.SetFloat(Fill, percentFill);
            
            onHealthIncrease?.Invoke((int)health);
        }
    }
}


// tween a number from 0 to total gems collected
// set the mat float
// when health per life ticks over 100 then play anim

// need ability to just tween a number and publish it
// and to tween
// need to raise event when