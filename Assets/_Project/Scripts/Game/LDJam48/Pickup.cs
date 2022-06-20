using System;
using UnityEngine;
using Util;
using Util.ObjPool;
using Util.Var;
using Util.Var.Events;
using Util.Var.Observe;

namespace LDJam48
{
    public class Pickup : MonoBehaviour
    {
        public int Value;
        [SerializeField] private float inactiveTime = .5f;
        [SerializeField] private string targetTag = "Player";

        [SerializeField] private AudioClipAssetGameEvent sfxChannel;
        [SerializeField] private AudioClipAsset clip;

        [SerializeField] private IntEventReference onPickupEvent;

        private Collider2D _col;
        private void Awake()
        {
            _col = GetComponent<Collider2D>();
        }

        private void OnEnable()
        {
            _col.enabled = false;
            this.ExecuteAfter(inactiveTime, () => _col.enabled = true);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(targetTag))
            {
                onPickupEvent.Raise(Value);
                DestroyPickup();
            }
        }

        private void DestroyPickup()
        {
            sfxChannel.Raise(clip);
            InstantiateEx.Destroy(gameObject);
        }
    }
}