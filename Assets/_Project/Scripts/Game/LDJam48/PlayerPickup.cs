using System;
using UnityEngine;
using Util;
using Util.Events;

namespace LDJam48
{
    public class PlayerPickup : MonoBehaviour
    {
        public int Value;
        [SerializeField] private float inactiveTime = .5f;

        [SerializeField] private AudioClipAssetGameEvent sfxChannel;
        [SerializeField] private AudioClipAsset clip;

        [SerializeField] private IntGameEvent onPickupEvent;

        private Collider2D _col;
        private void Start()
        {
            _col = GetComponent<Collider2D>();
            _col.enabled = false;
            this.ExecuteAfter(inactiveTime, () => _col.enabled = true);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                onPickupEvent.Raise(Value);
                DestroyPickup();
            }
        }

        private void DestroyPickup()
        {
            sfxChannel.Raise(clip);
            Destroy(gameObject);
        }
    }
}