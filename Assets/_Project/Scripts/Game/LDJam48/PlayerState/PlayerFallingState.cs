using UnityEngine;
using Util.Var;

namespace LDJam48.PlayerState
{
    [CreateAssetMenu(menuName = "Custom/PlayerState/Falling")]
    public class PlayerFallingState : PlayerState
    {
        [SerializeField] private float fallSpeed = 13;

        [SerializeField] private string anim = "player_fall";

        [SerializeField] private Vector2Reference maxVelocity;

        [SerializeField] private ShakeDefinition onLandShake;



        public override void OnEnter(PlayerState previous)
        {
            _machine.Context.Animator.Play(anim);

            maxVelocity.Value = new Vector2(maxVelocity.Value.x, fallSpeed);

            // _machine.Context.Rigidbody2D.velocity = new Vector2(_machine.Context.Rigidbody2D.velocity.x, -fallSpeed);

            _machine.Context.OnDashInput += OnDash;
        }

        public override void OnExit(PlayerState next)
        {
            base.OnExit(next);
            _machine.Context.OnDashInput -= OnDash;

            _machine.Context.CarriedYVel = _machine.Context.Rigidbody2D.velocity.y;

            if (next == _machine.States.Idle)
            {
                _machine.Context.FloorImpactEffectEvent.Raise(new ParticleEffectRequest
                {
                    Position = _machine.Context.BottomEffectPoint.position,
                    Rotation = _machine.Context.BottomEffectPoint.rotation,
                    Scale = _machine.Context.BottomEffectPoint.localScale
                });
                _machine.Context.ShakeEvent.Raise(onLandShake);
            }
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