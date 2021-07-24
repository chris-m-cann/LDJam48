using System;
using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using Random = UnityEngine.Random;

namespace LDJam48
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SlugBehaviour : MonoBehaviour
    {
        [SerializeField] private float speed;

        [SerializeField] private LayerMask blockers;
        [SerializeField] private float castDistance = .75f;
        [SerializeField] private float forwardCastOffset = .2f;
        
        private SpriteRenderer _sprite;

        private RaycastHit2D[] _hits = new RaycastHit2D[1];
        private Vector2 _dir = Vector2.right;

        private void Awake()
        {
            _sprite = GetComponent<SpriteRenderer>();
        }

        private void OnEnable()
        {
            var facing = Mathf.Sign(Random.value - .5f);
            _dir = transform.right * facing;
            _sprite.flipX = facing <= 0;
        }

        private void OnDrawGizmosSelected()
        {
            var forwardCastOrigin = transform.position - (forwardCastOffset * transform.up);
            Gizmos.color = Color.red;
            Gizmos.DrawRay(forwardCastOrigin, castDistance * _dir);

            var downCastOrigin = forwardCastOrigin + (Vector3) (castDistance * _dir);
            Gizmos.color = Color.green;
            Gizmos.DrawRay(downCastOrigin, castDistance * -transform.up);
        }

        private void FixedUpdate()
        {
            if (AnyBlockersInFront() || !IsFloorInFront())
            {
                _dir *= -1;
                
                // make sure we arnt just turning every update with nowhere to go
                if (!AnyBlockersInFront() && IsFloorInFront())
                {
                    _sprite.flipX = !_sprite.flipX;
                }


                return;
            }

            transform.Translate(_dir * (speed * Time.fixedDeltaTime), Space.World);
        }

        private bool AnyBlockersInFront()
        {
            var forwardCastOrigin = transform.position + (forwardCastOffset * -transform.up);
            return AnyBlockers(forwardCastOrigin, castDistance * _dir);
        }
        
        private bool IsFloorInFront()
        {
            var castOrigin = transform.position + (forwardCastOffset * -transform.up) + (Vector3)(castDistance * _dir);
            return AnyBlockers(castOrigin, castDistance * -transform.up);
        }
        
        private bool AnyBlockers(Vector2 castFrom, Vector2 castDir)
        {
            return Physics2D.RaycastNonAlloc(castFrom, castDir, _hits,
                castDistance, blockers) > 0;
        }
    }
}