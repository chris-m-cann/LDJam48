using System;
using Unity.VisualScripting;
using UnityEngine;
using Util;
using Util.Var;
using Util.Var.Events;

namespace LDJam48.PlayerState
{
    [CreateAssetMenu(menuName = "Custom/PlayerState/Special")]
    public class PlayerSpecialState : PlayerState
    {
        [SerializeField] private float speed = 10;
        [SerializeField] private float duration = .3f;
        [SerializeField] private string anim = "player_slam";
        [SerializeField] private AudioClipAsset sound;
        [SerializeField] private Vector2Reference maxVelocity;
        [SerializeField] private ShakeDefinition onLandShake;
        [SerializeField] private ParticleEffectRequestEventReference onLandEffect;
        [SerializeField] private ParticleEffectRequestEventReference slamEffect;


        [NonSerialized] private bool _idleNextFrame = false;

        public override void OnEnter(PlayerState previous)
        {
            _idleNextFrame = false;
            maxVelocity.Value = new Vector2(maxVelocity.Value.x, speed);
            _machine.Context.Rigidbody2D.velocity = new Vector2(0, -speed);
            _machine.Context.Animator.Play(anim);

            _machine.Context.MainCollider.enabled = false;
            _machine.Context.SlamCollider.enabled = true;

            slamEffect.Raise(new ParticleEffectRequest
            {
                Position = _machine.Context.SlamEffectPoint.position,
                Rotation = _machine.Context.SlamEffectPoint.rotation,
                Scale = _machine.Context.SlamEffectPoint.localScale
            });

            _machine.Context.SfxChannel.Raise(sound);

            _machine.Context.ExecuteAfter(duration, () =>
            {
                _machine.Context.MainCollider.enabled = true;
                _machine.Context.SlamCollider.enabled = false;
                if (_machine.CurrentState == this)
                {
                    _machine.CurrentState = _machine.States.Falling;
                }
            });

            _machine.Context.SlamParticles.GameObject().SetActive(true);
        }

        public override PlayerState OnUpdate()
        {
            if (_idleNextFrame)
            {
                return _machine.States.Idle;
            }

            return base.OnUpdate();
        }

        public override void OnExit(PlayerState next)
        {
            base.OnExit(next);

            _machine.Context.SlamParticles.GameObject().SetActive(false);
            _machine.Context.Rigidbody2D.velocity = new Vector2(0, _machine.Context.CarriedYVel);
            _machine.Context.SlamParticles.GameObject().SetActive(false);

            if (next == _machine.States.Idle)
            {
                onLandEffect.Raise(new ParticleEffectRequest
                {
                    Position = _machine.Context.BottomEffectPoint.position,
                    Rotation = _machine.Context.BottomEffectPoint.rotation,
                    Scale = _machine.Context.BottomEffectPoint.localScale
                });
                _machine.Context.ShakeEvent.Raise(onLandShake);
            }
        }

        public override PlayerState TransitionChecks()
        {
            if (_machine.Context.Contacts.HitFloorThisTurn)
            {
                return _machine.States.Idle;
            }

            if (_machine.Context.Contacts.IsOnFloor)
            {
                _idleNextFrame = true;
            }

            return null;
        }
    }
}