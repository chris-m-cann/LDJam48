using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Util;
using Util.Events;
using Util.Variable;

namespace LDJam48
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class PlayerDamageable : MonoBehaviour, IDamageable
    {
        [SerializeField] private ObservableIntVariable health;
        [SerializeField] private float invincibleTime;
        [SerializeField] private float invincibleFlashFreq;

        [SerializeField] private UnityEvent onZeroHealth;

        [SerializeField] private AudioClipAssetGameEvent sfxChannel;
        [SerializeField] private AudioClipAsset hitClip;
        [SerializeField] private AudioClipAsset dieClip;


        [SerializeField] private bool godmode;
        

        private SpriteRenderer _sprite;

        private bool _isInvincible;

        private void Awake()
        {
            _sprite = GetComponent<SpriteRenderer>();
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
                    sfxChannel.Raise(hitClip);
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