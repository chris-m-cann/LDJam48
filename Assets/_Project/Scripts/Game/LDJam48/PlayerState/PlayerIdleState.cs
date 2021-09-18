using UnityEngine;
using Util;
using Util.Var.Events;

namespace LDJam48.PlayerState
{
    [CreateAssetMenu(menuName = "Custom/PlayerState/Idle")]
    public class PlayerIdleState : PlayerState
    {
        [SerializeField] private string anim = "player_idle";
        [SerializeField] private string slamLandAnim = "player_slam_land";
        [SerializeField] private AudioClipAsset sound;
        [SerializeField] private ParticleEffectRequestEventReference regularLandEffect;


        public override void OnEnter(PlayerState previous)
        {
           
            _machine.Context.Rigidbody2D.velocity = Vector2.zero;

            _machine.Context.Animator.Play(anim);

            _machine.Context.SfxChannel.Raise(sound);
            regularLandEffect.Raise(new ParticleEffectRequest
            {
                Position = _machine.Context.BottomEffectPoint.position,
                Rotation = Quaternion.identity,
                Scale = Vector3.one
            });

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