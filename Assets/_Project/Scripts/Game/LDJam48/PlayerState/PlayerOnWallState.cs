using UnityEngine;
using Util.Var;

namespace LDJam48.PlayerState
{
    [CreateAssetMenu(menuName = "Custom/PlayerState/OnWall")]
    public class PlayerOnWallState : PlayerState
    {
        [SerializeField] private float minWallSpeed = 5;
        [SerializeField] private float maxWallSpeed = 10;
        [SerializeField] private string anim = "player_slide";
        [SerializeField] private Vector2Reference maxVelocity;


        private bool _isLeft = false;

        public override PlayerState OnEnter()
        {

            maxVelocity.Value = new Vector2(0, maxWallSpeed);

            var wallSpeed = Mathf.Min(-minWallSpeed, _machine.Context.CarriedYVel);

            _machine.Context.Rigidbody2D.velocity = new Vector2(0, wallSpeed);
            _machine.Context.Animator.Play(anim);

            _isLeft = _machine.Context.Contacts.IsOnLeftWall;
            _machine.Context.Sprite.flipX = !_isLeft;


            _machine.Context.OnDashInput += OnDash;

            return null;
        }

        public override void OnExit()
        {
            base.OnExit();

            _machine.Context.OnDashInput -= OnDash;
            _machine.Context.CarriedYVel = _machine.Context.Rigidbody2D.velocity.y;

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
            if (_machine.Context.Contacts.LeftLeftWallThisTurn || _machine.Context.Contacts.LeftRightWallThisTurn)
            {
                return _machine.States.Falling;
            }

            return null;
        }
    }
}