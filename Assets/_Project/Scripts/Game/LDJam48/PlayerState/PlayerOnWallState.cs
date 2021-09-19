using Unity.VisualScripting;
using UnityEngine;
using Util.Var;

namespace LDJam48.PlayerState
{
    [CreateAssetMenu(menuName = "Custom/PlayerState/OnWall")]
    public class PlayerOnWallState : PlayerState
    {
        [SerializeField] private float minWallSpeed = 5;
        [SerializeField] private float maxWallSpeed = 10;
        [SerializeField] private string anim = "player_wall_land";
        [SerializeField] private Vector2Reference maxVelocity;

        [SerializeField] private LayerMask wallMask;
        [SerializeField] private float boxcastSize = .5f;



        private bool _isLeft = false;

        public override void OnEnter(PlayerState previous)
        {

            maxVelocity.Value = new Vector2(0, maxWallSpeed);

            var wallSpeed = Mathf.Min(-minWallSpeed, _machine.Context.CarriedYVel);

            _machine.Context.Rigidbody2D.velocity = new Vector2(0, wallSpeed);
            _machine.Context.Animator.Play(anim);

            _isLeft = _machine.Context.Contacts.IsOnLeftWall;
            _machine.Context.Sprite.flipX = !_isLeft;

            StickToWall();

            EnableWallParticles(_machine.Context.WallSlideParticles);

            _machine.Context.OnDashInput += OnDash;
        }

        private void StickToWall()
        {
            // cast from farthest point toward wall
            var direction = Vector2.left;
            var projectionPoint = _machine.Context.RightProjectionPoint;
            var offset = .5f;

            if (!_isLeft)
            {
                direction.x += -1;
                projectionPoint = _machine.Context.LeftProjectionPoint;
                offset *= -1;
            }
            var hit = Physics2D.BoxCast(projectionPoint.position, boxcastSize * Vector2.one, 0f, direction, 10f, wallMask);

            if (hit.collider != null)
            {
                var pos = _machine.Context.transform.position;
                pos.x = hit.point.x + offset;
            }

        }

        private void EnableWallParticles(ParticleSystem wallSlideParticles)
        {
            var tform = wallSlideParticles.transform;
            tform.SetParent(_isLeft ? _machine.Context.LeftEffectPoint : _machine.Context.RightEffectPoint, false);
            wallSlideParticles.gameObject.SetActive(true);
        }

        public override void OnExit(PlayerState next)
        {
            base.OnExit(next);

            _machine.Context.OnDashInput -= OnDash;
            _machine.Context.CarriedYVel = _machine.Context.Rigidbody2D.velocity.y;

            _machine.Context.WallSlideParticles.gameObject.SetActive(false);

            maxVelocity.Value = new Vector2(100, maxWallSpeed);
        }

        private void OnDash(Vector2 dir)
        {
            if (_isLeft && dir.x > 0)
            {
                _machine.CurrentState = _machine.States.Dashing.Init(dir);
            }
            else if (!_isLeft && dir.x < 0)
            {
                _machine.CurrentState = _machine.States.Dashing.Init(dir);
            }
        }

        public override PlayerState TransitionChecks()
        {
            _machine.Context.UpdateContacts();
            // todo(chris) need to handle the case where we were on both walls but the one we were up against left
            // perhaps turn off the oposing walls detection collider whensliding? that way we will go into falling for a frame before "catching" the other wall?
            if (!_machine.Context.Contacts.IsOnLeftWall && !_machine.Context.Contacts.IsOnRightWall)
            {
                return _machine.States.Falling;
            }

            return null;
        }
    }
}