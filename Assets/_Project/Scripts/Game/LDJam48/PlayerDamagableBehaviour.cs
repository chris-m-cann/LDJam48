using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Util;
using Util.Var;
using Util.Var.Events;
using Util.Var.Observe;

namespace LDJam48
{
    public class PlayerDamagableBehaviour : DamagableBehaviour
    {
        [Tooltip("must be the same as used in [health]")]
        [SerializeField] private ObservableIntVariable healthObservable;
        
        [SerializeField] private AudioClipAssetGameEvent sfxChannel;
        [SerializeField] private AudioClipAsset hitClip;
        [SerializeField] private AudioClipAsset dieClip;
        
        [SerializeField] private ParticleEffectRequestEventReference hurtEffect;
        [SerializeField] private Volume volume;
        [SerializeField] [Range(0, 1)] private float on;
        [SerializeField] private float vignetteTime;

        [SerializeField] private Material hurtMaterial;
        [SerializeField] private float hurtTime;
        
        [SerializeField] private float invincibleTime;
        [SerializeField] private float invincibleFlashFreq;
        
        private SpriteRenderer _sprite;
        
        private float _prevVignetteIntensity;
        private Vector2 _prevVignetteCenter;
        private Material _prevMat;
     
        private void Awake()
        {
            _sprite = GetComponent<SpriteRenderer>();
            volume = FindObjectOfType<Volume>();
        }

        private void OnEnable()
        {
            healthObservable.Reset();
        }

        protected override void OnHurt()
        {
            hurtEffect.Raise(new ParticleEffectRequest
            {
                Position = transform.position,
                Rotation = transform.rotation,
                Scale = transform.localScale
            });
                    
            sfxChannel.Raise(hitClip);
                    
            if (volume.profile.TryGet(out Vignette vignette))
            {
                _prevVignetteIntensity = vignette.intensity.value;
                _prevVignetteCenter = vignette.center.value;
                vignette.intensity.value = on;
                vignette.center.value = Camera.main.WorldToViewportPoint(transform.position);
                this.ExecuteAfter(vignetteTime, () =>
                {
                    vignette.intensity.value = _prevVignetteIntensity;
                    vignette.center.value = _prevVignetteCenter;
                });
            }
                    
            _prevMat = _sprite.material;
            _sprite.material = hurtMaterial;
            
            this.ExecuteAfter(hurtTime, () =>
            {
                _sprite.material = _prevMat;
            });
            
            GoInvincible();
            
            base.OnHurt();
        }
        
        private void GoInvincible()
        {
            IsInvincible = true;

            StartCoroutine(CoFlash());
        }

        private IEnumerator CoFlash()
        {
            var begin = Time.time;
            var end = begin + invincibleTime;

            while (Time.time < end)
            {
                _sprite.enabled = !_sprite.enabled;
                // half as freq is flashes per sec and this is only a half flash
                yield return new WaitForSeconds(.5f / invincibleFlashFreq);
            }

            _sprite.enabled = true;
            IsInvincible = false;
        }

        protected override void OnDead()
        {
            sfxChannel.Raise(dieClip);
            base.OnDead();
        }
    }
}