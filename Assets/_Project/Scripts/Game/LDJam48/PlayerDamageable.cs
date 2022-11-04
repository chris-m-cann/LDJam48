using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Unity.XR.OpenVR;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using Util;
using Util.Var;
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

        [SerializeField] private Material hurtMaterial;
        [SerializeField] private Material resetMaterial;
        [SerializeField] private float hurtTime;

        [SerializeField] private GameObjectReference spotlight;
        [SerializeField] private float spotlightTime;
        

        [SerializeField] private bool godmode;

        private SpriteRenderer _sprite;

        private bool _isInvincible;
        private static readonly int ViewportPos = Shader.PropertyToID("_ViewportPos");

        private void Awake()
        {
            _sprite = GetComponent<SpriteRenderer>();
        }

        private void OnEnable()
        {
            health.Reset();
        }

        public void Damage(int amount, GameObject damager)
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
                    onHurt?.Invoke();
                    
                    hurtEffect.Raise(new ParticleEffectRequest
                    {
                        Position = transform.position,
                        Rotation = transform.rotation,
                        Scale = transform.localScale
                    });
                    
                    sfxChannel.Raise(hitClip);

                    if (spotlight != null && spotlight.Value != null)
                    {
                        StartCoroutine(CoSpotlight());

                    }

                    _sprite.material = hurtMaterial;

                    this.ExecuteAfter(hurtTime, () => { _sprite.material = resetMaterial; });
                    

                }
            }

            if (amount > 0)
            {
                GoInvincible();
            }
        }

        private IEnumerator CoSpotlight()
        {
            var go = spotlight.Value;
            go.SetActive(true);
            var image = go.GetComponent<Image>();
            image.enabled = true;

            var cam = Camera.main;
            var end = Time.time + spotlightTime;

            while (Time.time < end)
            {
                var pos = (Vector2)cam.WorldToViewportPoint(transform.position);
                image.material.SetVector(ViewportPos, pos);
                yield return null;
            }

            go.SetActive(false);
        }

        public void Kill(GameObject damager)
        {
            Damage(health.Value, damager);
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

        public void SetInvincible(bool invincible)
        {
            _isInvincible = invincible; // todo (chris) will this be overwritten by CoFlash on complete??
        }
    }
}