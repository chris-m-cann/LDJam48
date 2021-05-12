using UnityEngine;
using Util.Var;
using Random = UnityEngine.Random;

namespace LDJam48
{
    public class FollowerBehaviour : MonoBehaviour
    {
        [SerializeField] private GameObjectVariable target;

        [SerializeField] private Vector2 speed;
        [SerializeField] private float findRange;
        [SerializeField] private float loseRange;
        [SerializeField] private LayerMask blockers;
        [SerializeField] private float radius;
        [SerializeField] private float castDistance = .7f;
        [SerializeField] private float minApproach = .5f;
        [SerializeField] private float afterHitIdleTime = 1f;
        [SerializeField] private Vector2 initialDelay;


        private Transform _target;

        private bool _isIdle = true;
        private RaycastHit2D[] _hits = new RaycastHit2D[1];
        private float _idleCounter;
        private float _actualSpeed;

        private void Start()
        {
            _target = target.Value.transform;
            _idleCounter = Random.Range(initialDelay.x, initialDelay.y);
            _actualSpeed = Random.Range(speed.x, speed.y);
        }

        private void OnDrawGizmosSelected()
        {
            if (_isIdle)
            {
                Gizmos.DrawWireSphere(transform.position, findRange);
            }
            else
            {
                Gizmos.DrawWireSphere(transform.position, loseRange);
            }
        }


        private void Update()
        {
            if (_target == null)
            {
                Idle();
                return;
            }

            _idleCounter -= Time.deltaTime;
            if (_idleCounter > 0)
            {
                Idle();
                return;
            }

            var dist = Vector2.Distance(transform.position, _target.position);

            if (_isIdle && dist < findRange)
            {
                // enter chase state
                _isIdle = false;
            }

            if (!_isIdle && dist > loseRange)
            {
                // end chase state
                _isIdle = true;
            }

            if (_isIdle)
            {
                Idle();
            }
            else
            {
                _idleCounter = 0;
                Chase();
            }
        }

        private void Idle()
        {

        }


        private void Chase()
        {

            var dir = _target.position - transform.position;

            // pause after hitting the player to give them a chance to reposition
            if (dir.magnitude < minApproach)
            {
                _isIdle = true;
                _idleCounter = afterHitIdleTime;
                return;
            }



            // if can move to player then do
            dir.Normalize();

            var hits = Physics2D.CircleCastNonAlloc(transform.position, radius, dir, _hits, castDistance, blockers);
            if (hits == 0)
            {
                transform.Translate(dir * speed * Time.deltaTime);
                return;
            }

            // if cant go direct, try and just go vertical
            var vertical = Vector2.up * Mathf.Sign(dir.y);

            hits = Physics2D.CircleCastNonAlloc(transform.position, radius, vertical, _hits, castDistance, blockers);

            if (hits == 0)
            {
                transform.Translate(vertical * speed * Time.deltaTime);
                return;
            }

            // if cant go direct or vertical try and go towards horizontally
            var horizontal = Vector2.right * Mathf.Sign(dir.x);

            hits = Physics2D.CircleCastNonAlloc(transform.position, radius, horizontal, _hits, castDistance, blockers);

            if (hits == 0)
            {
                transform.Translate(horizontal * (_actualSpeed * Time.deltaTime));
                return;
            }
        }
    }
}