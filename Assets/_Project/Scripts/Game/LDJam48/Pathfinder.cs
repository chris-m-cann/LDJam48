using System;
using UnityEngine;

namespace LDJam48
{
    public class Pathfinder : MonoBehaviour
    {
        [SerializeField] private float minApproach;
        [SerializeField] private float speed;
        [SerializeField] private float radius;
        [SerializeField] private float castDistance;
        [SerializeField] private LayerMask blockers;

        public event Action OnAtTarget;


        private Transform _target;
        private RaycastHit2D[] _hits = new RaycastHit2D[1];


        public void SetTarget(Transform target)
        {
            _target = target;
        }

        private void Update()
        {
            if (_target == null) return;

            Chase();
        }

        private void Chase()
        {

            var dir = _target.position - transform.position;

            // pause after hitting the player to give them a chance to reposition
            if (dir.magnitude < minApproach)
            {
                OnAtTarget?.Invoke();
                _target = null;
                return;
            }


            // if can move to player then do
            dir.Normalize();

            var hits = Physics2D.CircleCastNonAlloc(transform.position, radius, dir, _hits, castDistance, blockers);
            if (hits == 0)
            {
                transform.Translate(dir * (speed * Time.deltaTime));
                return;
            }

            // if cant go direct, try and just go vertical
            var vertical = Vector2.up * Mathf.Sign(dir.y);

            hits = Physics2D.CircleCastNonAlloc(transform.position, radius, vertical, _hits, castDistance, blockers);

            if (hits == 0)
            {
                transform.Translate(vertical * (speed * Time.deltaTime));
                return;
            }

            // if cant go direct or vertical try and go towards horizontally
            var horizontal = Vector2.right * Mathf.Sign(dir.x);

            hits = Physics2D.CircleCastNonAlloc(transform.position, radius, horizontal, _hits, castDistance, blockers);

            if (hits == 0)
            {
                transform.Translate(horizontal * (speed * Time.deltaTime));
                return;
            }
        }
    }
}