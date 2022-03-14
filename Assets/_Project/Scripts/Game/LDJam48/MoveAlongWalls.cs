using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace LDJam48
{
    
    public class MoveAlongWalls : MonoBehaviour
    {
        [SerializeField] private float speed;

        [SerializeField] private LayerMask blockers;
        [SerializeField] private LayerMask turnAroundBlockers;
        [SerializeField] private float forwardCastDistance = .75f;
        [SerializeField] private float downCastDistance = .75f;
        [SerializeField] private Vector2 forwardCastOffset = Vector2.zero;
        [SerializeField] private Vector2 downCastOffset = Vector2.zero;
        [SerializeField] private float lerpTime = .2f;

        [SerializeField] private SpriteRenderer sprite;
        [SerializeField] private bool spriteFacesRight = true;
        

        private RaycastHit2D[] _hits = new RaycastHit2D[1];
        private Vector2 _dir = Vector2.right;
        private Vector2 _down = Vector2.right;
        private float _facing;

        private bool _grounded;
        private bool _clearFront = true;
        private bool _lerping = false;
        private bool _paused;

        private void Awake()
        {
            _facing = Mathf.Sign(Random.value - .5f);
            _dir = transform.right * _facing;
            _down = -transform.up;
            sprite.flipX = spriteFacesRight ? _facing <= 0 : _facing >= 0;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, forwardCastDistance * _dir);

            Gizmos.color = Color.green;
            var down = Application.isPlaying ? (Vector3)_down : -transform.up;
            Gizmos.DrawRay(transform.position, downCastDistance * down);
            
        }

        private void FixedUpdate()
        {
            if (!enabled) return;
            
            if (AnyBlockers(transform.position, _dir, forwardCastDistance, turnAroundBlockers))
            {
                _dir = new Vector2(-_dir.x, -_dir.y);
                sprite.flipX = !sprite.flipX;
                _facing *= -1;
                return;
            }
            
            var blockedFront = AnyBlockersInFront();
            var onFloor = IsFloorInFront();

            // once we have rotated down t allow rotation again until ground has been found
            if (_grounded)
            {
                if (!onFloor)
                {
                    transform.Translate(.5f * _dir.normalized + .5f * _down.normalized, Space.World);
                    _dir = Quaternion.Euler(0, 0, _facing * -90) * _dir;
                    _down = Quaternion.Euler(0, 0, _facing * -90) * _down;
                    transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z - _facing * 90);

                    _grounded = false;
                }
            }
            
            if (onFloor)
            {
                _grounded = true;
            }


            // if (_clearFront)
            {
                if (blockedFront)
                {
                    
                    _dir = Quaternion.Euler(0, 0, _facing * 90) * _dir;
                    _down = Quaternion.Euler(0, 0, _facing * 90) * _down;
                    transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z + _facing * 90);

                    _clearFront = false;
                }
            }

            if (!blockedFront)
            {
                _clearFront = true;
            }
            
            
            transform.Translate(_dir * (speed * Time.fixedDeltaTime), Space.World);
        }

        private bool AnyBlockersInFront()
        {
            return AnyBlockers(transform.position, _dir, forwardCastDistance);
        }

        
        private bool IsFloorInFront()
        {
            return AnyBlockers(transform.position, _down, downCastDistance);
        }

        
        private bool AnyBlockers(Vector2 castFrom, Vector2 castDir, float castDistance)
        {
            return AnyBlockers(castFrom, castDir, castDistance, blockers);
        }
        
        private bool AnyBlockers(Vector2 castFrom, Vector2 castDir, float castDistance, LayerMask layers)
        {
            return Physics2D.RaycastNonAlloc(castFrom, castDir, _hits, castDistance, layers) > 0;
        }
    }
    
}