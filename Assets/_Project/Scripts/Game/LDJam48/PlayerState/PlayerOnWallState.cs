using UnityEngine;

namespace LDJam48.PlayerState
{
    [CreateAssetMenu(menuName = "Custom/PlayerState/OnWall")]
    public class PlayerOnWallState : PlayerState
    {
        [SerializeField] private float wallSpeed = 10;
        [SerializeField] private string anim = "player_slide";


        private bool _isLeft = false;

        public override void OnEnter(StateMachine machine)
        {
            base.OnEnter(machine);

            machine.Context.Rigidbody2D.velocity = new Vector2(0, -wallSpeed);
            machine.Context.Animator.Play(anim);

            _isLeft = machine.Context.Contacts.IsOnLeftWall;
            machine.Context.Sprite.flipX = !_isLeft;


            _machine.Context.OnDashInput += OnDash;
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

        public override void TransitionChecks()
        {
            base.TransitionChecks();

            if (_machine.Context.Contacts.LeftLeftWallThisTurn || _machine.Context.Contacts.LeftRightWallThisTurn)
            {
                _machine.CurrentState = _machine.States.Falling;
            }
        }
    }
}