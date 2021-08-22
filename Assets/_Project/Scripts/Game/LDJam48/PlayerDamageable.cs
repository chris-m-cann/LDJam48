using System.Collections;
using Unity.XR.OpenVR;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Util;
using Util.Var.Events;
using Util.Var.Observe;

namespace LDJam48
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class PlayerDamageable : MonoBehaviour, IDamageable
    {
        [SerializeField] private ObservableIntVariable health;
        [SerializeField] private float invincibleTime;
        [SerializeField] private float invincibleFlashFreq;

        [SerializeField] private UnityEvent onHurt;
        [SerializeField] private UnityEvent onZeroHealth;

        [SerializeField] private AudioClipAssetGameEvent sfxChannel;
        [SerializeField] private AudioClipAsset hitClip;
        [SerializeField] private AudioClipAsset dieClip;

        [SerializeField] private ParticleEffectRequestEventReference hurtEffect;

        [SerializeField] private Volume volume;
        [SerializeField] [Range(0, 1)] private float on;
        [SerializeField] private float vignetteTime;

        [SerializeField] private Material hurtMaterial;
        [SerializeField] private float hurtTime;

        [SerializeField] private bool godmode;

        private SpriteRenderer _sprite;

        private bool _isInvincible;
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
            health.Reset();
        }

        public void Damage(int amount)
        {
            if (_isInvincible) return;

            if (!godmode)
            {
                var afterDamage = health.Value - amount;
                health.Value = Mathf.Max(afterDamage, 0);

                if (health.Value == 0)
                {
                    sfxChannel.Raise(dieClip);
                    onZeroHealth.Invoke();
                }
                else
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
                    
                    onHurt?.Invoke();
                }
            }

            if (amount > 0)
            {
                GoInvincible();
            }
        }

        private void GoInvincible()
        {
            _isInvincible = true;

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
            _isInvincible = false;
        }
    }
}