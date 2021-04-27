using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace LDJam48
{
    public class ToAndFroBehaviour : MonoBehaviour
    {
        [SerializeField] private float speed;

        [SerializeField] private LayerMask blockers;
        [SerializeField] private float castDistance = .75f;


        private RaycastHit2D[] _hits = new RaycastHit2D[1];
        private Vector2 _dir;
        private void Start()
        {
            // pick a random dir to start off in
            _dir = transform.right * Mathf.Sign(Random.value - .5f);
        }

        private void Update()
        {
            Debug.DrawRay(transform.position, _dir, Color.red, 1f);
            var blockerInFront = AnyBlockers(transform.position, _dir);

            if (blockerInFront)
            {
                _dir *= -1;
                return;
            }

            transform.Translate(_dir * (speed * Time.deltaTime), Space.World);
        }

        private bool AnyBlockers(Vector2 castFrom, Vector2 castDir)
        {
            return Physics2D.RaycastNonAlloc(castFrom, castDir, _hits,
                castDistance, blockers) > 0;
        }
    }
}