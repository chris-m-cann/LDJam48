using System;
using UnityEngine;

namespace LDJam48
{
    
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteFaceVelocity : MonoBehaviour
    {
        private SpriteRenderer _sprite;
        private Rigidbody2D _rigidbody;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _sprite = GetComponent<SpriteRenderer>();
        }

        private void FixedUpdate()
        {
            _sprite.flipX = _rigidbody.velocity.x < 0;
        }
    }
}