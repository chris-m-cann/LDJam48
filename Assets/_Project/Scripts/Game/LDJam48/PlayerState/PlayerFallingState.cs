using UnityEngine;

namespace LDJam48.PlayerState
{
    [CreateAssetMenu(menuName = "Custom/PlayerState/Falling")]
    public class PlayerFallingState : PlayerState
    {
        [SerializeField] private float fallSpeed = 13;

        [SerializeField] private string anim = "player_fall";

        public override void OnEnter(StateMachine machine)
        {
            base.OnEnter(machine);

            if (machine.Context.Contacts.IsOnLeftWall || machine.Context.Contacts.IsOnRightWall)
            {
                machine.CurrentState = machine.States.OnWall;
                return;
            }

            machine.Context.Animator.Play(anim);

            machine.Context.Rigidbody2D.velocity = new Vector2(machine.Context.Rigidbody2D.velocity.x, -fallSpeed);

            _machine.Context.OnDashInput += OnDash;
        }

        public override void OnExit()
        {
            base.OnExit();
            _machine.Context.OnDashInput -= OnDash;
        }

        private void OnDash(Vector2 dir) => _machine.CurrentState = _machine.States.Dashing.Init(dir);
        public override void TransitionChecks()
        {
            base.TransitionChecks();

            if (_machine.Context.Contacts.HitFloorThisTurn)
            {
                _machine.CurrentState = _machine.States.Idle;
                return;
            }

            if (_machine.Context.Contacts.HitLeftWallThisTurn || _machine.Context.Contacts.HitRightWallThisTurn)
            {
                _machine.CurrentState = _machine.States.OnWall;
                return;
            }
        }
    }
}