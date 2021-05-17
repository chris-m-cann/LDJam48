using UnityEngine;

namespace LDJam48.PlayerState
{
    [CreateAssetMenu(menuName = "Custom/PlayerState/Falling")]
    public class PlayerFallingState : PlayerState
    {
        [SerializeField] private float fallSpeed = 13;

        [SerializeField] private string anim = "player_fall";

        public override PlayerState OnEnter()
        {
            _machine.Context.Animator.Play(anim);

            _machine.Context.Rigidbody2D.velocity = new Vector2(_machine.Context.Rigidbody2D.velocity.x, -fallSpeed);

            _machine.Context.OnDashInput += OnDash;

            return null;
        }

        public override void OnExit()
        {
            base.OnExit();
            _machine.Context.OnDashInput -= OnDash;
        }

        private void OnDash(Vector2 dir) => _machine.CurrentState = _machine.States.Dashing.Init(dir);
        public override PlayerState TransitionChecks()
        {
            if (_machine.Context.Contacts.IsOnFloor)
            {
                return _machine.States.Idle;
            }

            if (_machine.Context.Contacts.IsOnLeftWall || _machine.Context.Contacts.IsOnRightWall)
            {
                return _machine.States.OnWall;
            }

            return null;
        }
    }
}