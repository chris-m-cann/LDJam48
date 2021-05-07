using System;
using UnityEngine;
using Util.Var;

namespace LDJam48
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class FaceTarget : MonoBehaviour
    {
        [SerializeField] private GameObjectVariable initialTarget;
        private Transform _target;
        private SpriteRenderer _sprite;

        private void Awake()
        {
            _sprite = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            _target = initialTarget?.Value?.transform;
        }

        private void Update()
        {
            if (_target == null) return;

            var diff = _target.position.x - transform.position.x;
            _sprite.flipX = diff < 0;
        }

        public void SetTarget(Transform target) =>_target = target;
    }
}