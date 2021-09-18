using System;
using System.Collections;
using DG.Tweening;
using LDJam48.LevelGen;
using UnityEngine;
using UnityEngine.InputSystem;
using Util;
using Util.Var;
using Util.Var.Events;
using Util.Var.Observe;
using Void = Util.Void;

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
        [SerializeField] private SpriteRenderer ghostPrefab;
        [SerializeField] private float[] ghostDelays;
        

        [SerializeField] private bool displayTrail = true;
        [SerializeField] private VoidGameEvent dashAnimEvent;
        [SerializeField] private LayerMask wallMask;
        [SerializeField] private AnimationCurve dashCurve;
        [SerializeField] private float dashSpeedMax;
        [SerializeField] private float dashSpeedMin;
        [SerializeField] private float leftWallX;
        [SerializeField] private float rightWallX;
        [SerializeField] private float boxcastSize = .5f;

        [SerializeField] private ParticleEffectRequestEventReference onWalHit;
        
        private Vector2 _direction;
        private float _prevGravity = 1f;
        private bool _wasOnWall = false;
        private Vector2 _startPos;
        private Coroutine _coroutine;
        private Coroutine _dashCoroutine;
        private Tweener _dashTweener;
        private float _endX;
        private bool _goingToWall = false;

        public PlayerDashingState Init(Vector2 direction)
        {
            _direction = direction.normalized;
            return this;
        }

        public override void OnEnter(PlayerState previous)
        {
            Debug.Log($"Entering dash at x pos = {_machine.Context.transform.position.x}");
            
            _machine.Context.CarriedYVel = _machine.Context.Rigidbody2D.velocity.y;
            _prevGravity = _machine.Context.Rigidbody2D.gravityScale;
            
            _machine.Context.Rigidbody2D.gravityScale = 0f;
            _machine.Context.Rigidbody2D.velocity = Vector2.zero;
            
            
            var isLeft = _direction.x < 0;
            _wasOnWall = _machine.Context.Contacts.IsOnLeftWall || _machine.Context.Contacts.IsOnRightWall;
            _machine.Context.Sprite.flipX = isLeft;
            var effectTransform = isLeft ? _machine.Context.RightEffectPoint : _machine.Context.LeftEffectPoint;
            _startPos = effectTransform.position;


            dashAnimEvent.OnEventTrigger += StartDash;
            // _machine.Context.Rigidbody2D.velocity = dashSpeed * _direction;
            _machine.Context.Animator.Play(anim);


            _machine.Context.MainCollider.enabled = false;
            _machine.Context.SlashCollider.enabled = true;


            _machine.Context.OnSlamInput += OnSlam;

            // Debug.Log($"OnEnter Setting actionMap = {actionMap}");
            activeActionMap.Value = actionMap;
            
            
            _coroutine = _machine.Context.StartCoroutine(ShowGhosts());
        }

        private void StartDash(Void v)
        {
            Debug.Log($"Starting dash at x pos = {_machine.Context.transform.position.x}");
            var isLeft = _direction.x < 0;


            _machine.Context.SfxChannel.Raise(sound);

            var effectTransform = isLeft ? _machine.Context.RightEffectPoint : _machine.Context.LeftEffectPoint;

            if (_wasOnWall)
            {
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

            var castPoint = _machine.Context.LeftProjectionPoint;
            var finalX = leftWallX;
            var offset = .5f;
            if (_direction.x > 0)
            {
                castPoint = _machine.Context.RightProjectionPoint;
                finalX = rightWallX;
                offset *= -1;
            }

            var hit = Physics2D.BoxCast(castPoint.position, boxcastSize * Vector2.one, 0f, _direction, rightWallX - leftWallX, wallMask);
            var hitX = finalX;

            if (hit.collider)
            {
                hitX = hit.point.x;
            }
            _dashCoroutine = _machine.Context.StartCoroutine(CoDash(hitX + offset));
            Debug.DrawLine(castPoint.position, new Vector3(hitX, castPoint.position.y), Color.red, 1f);
        }

        private IEnumerator CoDash(float finalX)
        {
            var rb = _machine.Context.Rigidbody2D;
            var vel = rb.velocity;

            var maxLevelWidth = rightWallX - leftWallX;
            var t = _machine.Context.transform;
            var initialX = t.position.x;

            var stillGoing = true;
            while (stillGoing)
            {
                var normalised = Mathf.Abs(t.position.x - initialX) / maxLevelWidth;
                vel.x = (dashSpeedMax - dashSpeedMin) * dashCurve.Evaluate(normalised) + dashSpeedMin;
                vel *= _direction.x;
                rb.velocity = vel;
                yield return null;

                if (_direction.x > 0)
                {
                    stillGoing = t.position.x < finalX;
                }
                else
                {
                    stillGoing = t.position.x > finalX;
                }
            }

            t.position = new Vector3(finalX, t.position.y, t.position.z);
            _machine.CurrentState = _machine.States.OnWall;
        }

        public override void OnExit(PlayerState next)
        {
            base.OnExit(next);
            _machine.Context.Rigidbody2D.gravityScale = _prevGravity;
            _dashTweener?.Kill();

            if (_dashCoroutine != null)
            {
                _machine.Context.StopCoroutine(_dashCoroutine);
            }

            _machine.Context.StopCoroutine(_coroutine);

            dashAnimEvent.OnEventTrigger -= StartDash;
            if (displayTrail)
            {
                _machine.Context.DashTrailEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            }

            _machine.Context.MainCollider.enabled = true;
            _machine.Context.SlashCollider.enabled = false;
            _machine.Context.OnSlamInput -= OnSlam;

            activeActionMap.Value = onExitActionMap;

            _machine.Context.Rigidbody2D.gravityScale = _prevGravity;

            if (next == _machine.States.OnWall)
            {
                // kick off the on a wall puff
                var t = _direction.x < 0 ? _machine.Context.LeftEffectPoint : _machine.Context.RightEffectPoint;
                var onWallRequest = new ParticleEffectRequest
                {
                    Position = t.position,
                    Rotation = t.localRotation,
                    Scale = t.localScale
                };
                _machine.Context.WallImpactEffectEvent.Raise(onWallRequest);
                onWalHit.Raise(onWallRequest);
                _machine.Context.ShakeEvent.Raise(onWallLandShake);
            }
        }

        private void OnSlam() => _machine.CurrentState = _machine.States.Slamming;

        public override PlayerState TransitionChecks()
        {
            if (_machine.Context.Contacts.HitFloorThisTurn)
            {
                return _machine.States.Idle;
            }

            return null;
        }

        IEnumerator ShowGhosts()
        {
            foreach (var delay in ghostDelays)
            {
                yield return new WaitForSeconds(delay);
                var ghost = Instantiate(ghostPrefab, _machine.Context.transform.position, Quaternion.identity);
                var playerSprite = _machine.Context.Sprite;
                ghost.sprite = playerSprite.sprite;
                ghost.flipX = playerSprite.flipX;
            }
        }
    }
}