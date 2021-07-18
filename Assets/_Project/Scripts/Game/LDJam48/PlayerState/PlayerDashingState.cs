using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using Util;
using Util.Var.Events;
using Util.Var.Observe;

namespace LDJam48.PlayerState
{
    [CreateAssetMenu(menuName = "Custom/PlayerState/Dashing")]
    public class PlayerDashingState : PlayerState
    {
        [SerializeField] private float dashSpeed = 15;
        [SerializeField] private string anim = "player_slice";
        [SerializeField] private string actionMap = "Dashing";
        [SerializeField] private string onExitActionMap = "NotDashing";
        [SerializeField] private AudioClipAsset sound;
        [SerializeField] private ObservableStringVariable activeActionMap;
        [SerializeField] private ShakeDefinition onDashShake;
        [SerializeField] private ShakeDefinition onWallLandShake;
        [SerializeField] private Vector3 trailOffset;
        [SerializeField] private Vector3 frontTrailOffset;
        [SerializeField] private float frontTrailDelay;

        [SerializeField] private bool displayTrail = true;
        [SerializeField] private VoidGameEvent dashAnimEvent;




        private Vector2 _direction;
        private float _prevGravity = 1f;
        private bool _wasOnWall = false;
        private Vector2 _startPos;

        public PlayerDashingState Init(Vector2 direction)
        {
            _direction = direction.normalized;
            return this;
        }

        public override void OnEnter(PlayerState previous)
        {
            var isLeft = _direction.x < 0;
            _wasOnWall = _machine.Context.Contacts.IsOnLeftWall || _machine.Context.Contacts.IsOnRightWall;
            _machine.Context.Sprite.flipX = isLeft;
            var effectTransform = isLeft ? _machine.Context.RightEffectPoint : _machine.Context.LeftEffectPoint;
            _startPos = effectTransform.position;

            _machine.Context.CarriedYVel = _machine.Context.Rigidbody2D.velocity.y;

            dashAnimEvent.OnEventTrigger += StartDash;
            _machine.Context.Rigidbody2D.velocity = dashSpeed * _direction;
            _prevGravity = _machine.Context.Rigidbody2D.gravityScale;
            _machine.Context.Rigidbody2D.gravityScale = 0f;
            _machine.Context.Animator.Play(anim);


            _machine.Context.MainCollider.enabled = false;
            _machine.Context.SlashCollider.enabled = true;


            _machine.Context.OnSlamInput += OnSlam;

            // Debug.Log($"OnEnter Setting actionMap = {actionMap}");
            activeActionMap.Value = actionMap;
        }

        private void StartDash(Void v)
        {
            var isLeft = _direction.x < 0;



            _machine.Context.SfxChannel.Raise(sound);

            var effectTransform = isLeft ? _machine.Context.RightEffectPoint : _machine.Context.LeftEffectPoint;

            if (_wasOnWall)
            {
                Debug.Log("Firing of dash effect");
                _machine.Context.DashEffectEvent.Raise(new ParticleEffectRequest
                {
                    Position = _startPos,
                    Rotation = effectTransform.rotation,
                    Scale = effectTransform.localScale
                });

                _machine.Context.ShakeEvent.Raise(onDashShake);
            }

            // var trailTransform = isLeft ? _machine.Context.LeftEffectPoint : _machine.Context.RightEffectPoint;

            if (displayTrail)
            {
                var flip = isLeft ? 1 : -1;

                _machine.Context.DashTrailEffect.transform.SetParent(effectTransform, false);
                _machine.Context.DashTrailEffect.transform.localPosition = trailOffset;
                _machine.Context.DashTrailEffect.transform.localScale = new Vector3(flip, 1, 1f);
                _machine.Context.DashTrailEffect.gameObject.SetActive(true);
                _machine.Context.DashTrailEffect.Play(true);
            }
        }

        public override void OnExit(PlayerState next)
        {
            base.OnExit(next);

            dashAnimEvent.OnEventTrigger -= StartDash;
            _machine.Context.DashTrailEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

            _machine.Context.MainCollider.enabled = true;
            _machine.Context.SlashCollider.enabled = false;
            _machine.Context.OnSlamInput -= OnSlam;

            activeActionMap.Value = onExitActionMap;

            _machine.Context.Rigidbody2D.gravityScale = _prevGravity;

            if (next == _machine.States.OnWall)
            {
                // kick off the on a wall puff
                var t = _direction.x < 0 ? _machine.Context.LeftEffectPoint : _machine.Context.RightEffectPoint;
                _machine.Context.WallImpactEffectEvent.Raise(new ParticleEffectRequest
                {
                    Position = t.position,
                    Rotation = t.rotation,
                    Scale = t.localScale
                });
                _machine.Context.ShakeEvent.Raise(onWallLandShake);
            }
        }

        private void OnSlam() => _machine.CurrentState = _machine.States.Slamming;

        public override PlayerState TransitionChecks()
        {
            if (_machine.Context.Contacts.HitLeftWallThisTurn ||
                _machine.Context.Contacts.HitRightWallThisTurn)
            {
                return _machine.States.OnWall;
            }

            return null;
        }
    }
}