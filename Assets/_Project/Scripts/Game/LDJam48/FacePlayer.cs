using System;
using UnityEngine;

namespace LDJam48
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class FacePlayer : MonoBehaviour
    {
        private Transform _player;
        private SpriteRenderer _sprite;

        private void Awake()
        {
            _sprite = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            _player = FindObjectOfType<PlayerController>()?.transform;
        }

        private void Update()
        {
            if (_player == null) return;

            var diff = _player.position.x - transform.position.x;
            _sprite.flipX = diff < 0;
        }
    }
}