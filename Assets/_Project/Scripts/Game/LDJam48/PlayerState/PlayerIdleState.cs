using UnityEngine;
using Util;

namespace LDJam48.PlayerState
{
    [CreateAssetMenu(menuName = "Custom/PlayerState/Idle")]
    public class PlayerIdleState : PlayerState
    {
        [SerializeField] private string anim = "player_idle";
        [SerializeField] private string slamLandAnim = "player_slam_land";
        [SerializeField] private AudioClipAsset sound;


        public override void OnEnter(PlayerState previous)
        {
            _machine.Context.Animator.Play(anim);
            _machine.Context.Rigidbody2D.velocity = Vector2.zero;

            _machine.Context.SfxChannel.Raise(sound);

            _machine.Context.OnDashInput += OnDash;
        }

        public override void OnExit(PlayerState next)
        {
            base.OnExit(next);

            _machine.Context.OnDashInput -= OnDash;
        }

        private void OnDash(Vector2 dir) => _machine.CurrentState = _machine.States.Dashing.Init(dir);
    }
}