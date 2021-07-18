using System;
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


        private SpriteRenderer _sprite;

        private RaycastHit2D[] _hits = new RaycastHit2D[1];
        private Vector2 _dir;

        private void Awake()
        {
            _sprite = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            // pick starting orientation
            if (AnyBlockers(transform.position, Vector2.left))
            {
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, -90));
            } else if (AnyBlockers(transform.position, Vector2.right))
            {
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
            } else if (AnyBlockers(transform.position, Vector2.up))
            {
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
            } else if (AnyBlockers(transform.position, Vector2.down))
            {
                Destroy(gameObject);
                return;
            }

            // pick a random dir to start off in
            var facing =Mathf.Sign(Random.value - .5f);

            _dir = transform.right * facing;

            if (facing > 0)
            {
                // todo(chris) this makes no sense to me why this is this way round on mobile?
                _sprite.flipX = true;
            }
        }

        private void FixedUpdate()
        {
            var blockerInFront = AnyBlockers(transform.position, _dir);

            var runOutOfGround =
                !AnyBlockers((Vector2) transform.position + _dir * castDistance, -transform.up);

            if (blockerInFront || runOutOfGround)
            {

                _dir *= -1;

                blockerInFront = AnyBlockers(transform.position, _dir);

                runOutOfGround =
                    !AnyBlockers((Vector2) transform.position + _dir * castDistance, -transform.up);

                // make sure we arnt just turning every round if nowhere to go
                if (!runOutOfGround && !blockerInFront)
                {
                    _sprite.flipX = !_sprite.flipX;
                }


                return;
            }

            transform.Translate(_dir * (speed * Time.fixedDeltaTime), Space.World);
        }

        private bool AnyBlockers(Vector2 castFrom, Vector2 castDir)
        {
            castFrom -= (Vector2)transform.up * .3f; // correct for being so low
            return Physics2D.RaycastNonAlloc(castFrom, castDir, _hits,
                castDistance, blockers) > 0;
        }
    }
}