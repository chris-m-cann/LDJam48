using UnityEngine;
using Util;

namespace LDJam48.PlayerState
{
    [CreateAssetMenu(menuName = "Custom/PlayerState/Idle")]
    public class PlayerIdleState : PlayerState
    {
        [SerializeField] private string anim = "player_idle";
        [SerializeField] private AudioClipAsset sound;


        public override void OnEnter(StateMachine machine)
        {
            base.OnEnter(machine);

            machine.Context.Animator.Play(anim);
            machine.Context.Rigidbody2D.velocity = Vector2.zero;

            _machine.Context.SfxChannel.Raise(sound);

            machine.Context.OnDashInput += OnDash;
        }

        public override void OnExit()
        {
            base.OnExit();

            _machine.Context.OnDashInput -= OnDash;
        }

        private void OnDash(Vector2 dir) => _machine.CurrentState = _machine.States.Dashing.Init(dir);
    }
}