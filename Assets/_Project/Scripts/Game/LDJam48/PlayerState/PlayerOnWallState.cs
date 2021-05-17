using UnityEngine;

namespace LDJam48.PlayerState
{
    [CreateAssetMenu(menuName = "Custom/PlayerState/OnWall")]
    public class PlayerOnWallState : PlayerState
    {
        [SerializeField] private float wallSpeed = 10;
        [SerializeField] private string anim = "player_slide";


        private bool _isLeft = false;

        public override PlayerState OnEnter()
        {
            _machine.Context.Rigidbody2D.velocity = new Vector2(0, -wallSpeed);
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