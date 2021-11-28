using System;
using UnityEngine;
using Util;
using Util.Var;
using Util.Var.Events;

namespace LDJam48.StateMachine.Player.Action
{
    [System.Serializable]
    public class PlayerSlamStateAction : StateAction
    {
        public float speed = 10;
        public float duration = .3f;
        public string anim = "player_slam";
        public AudioClipAsset sound;
        public Vector2Reference maxVelocity;
        public ShakeDefinition onLandShake;
        public ParticleEffectRequestEventReference onLandEffect;
        public ParticleEffectRequestEventReference slamEffect;
        public int OngoingSlamEffectId;
        public int SlamEffectId;
        public int SlamSoundId;
        public FloatVariable CarriedYVel;
        protected override IStateAction BuildRuntimeImpl()
        {
            return new PlayerSlamStateActionRuntime();
        }
    }

    public class PlayerSlamStateActionRuntime : BaseStateActionRuntime<PlayerSlamStateAction>
    {
        private Rigidbody2D _rigidbody;
        private PlayerColliders _colliders;
        private ParticlesBehaviour _particles;
        private SoundsBehaviour _sounds;

        public override void OnAwake(StateMachineBehaviour machine)
        {
            base.OnAwake(machine);

            _rigidbody = machine.GetComponent<Rigidbody2D>();
            _colliders = machine.GetComponent<PlayerColliders>();
            _particles = machine.GetComponent<ParticlesBehaviour>();
            _sounds = machine.GetComponent<SoundsBehaviour>();
        }

        public override void OnStateEnter()
        {
            base.OnStateEnter();

            _source.maxVelocity.Value = new Vector2(_source.maxVelocity.Value.x, _source.speed);
            _rigidbody.velocity = new Vector2(0, -_source.speed);

            _colliders.Main.enabled = false;
            _colliders.Slam.enabled = true;

            _particles.PlayEffect(_source.SlamEffectId);
            _particles.PlayEffect(_source.OngoingSlamEffectId);

            _sounds.PlaySound(_source.SlamSoundId);
        }


        public override void OnStateExit()
        {
            base.OnStateExit();
            _colliders.Slam.enabled = false;
            _colliders.Main.enabled = true;
            _particles.StopEffect(_source.OngoingSlamEffectId);


            _rigidbody.velocity = new Vector2(0, _source.CarriedYVel.Value);
        }
    }

    // on enter anim
    // transition out after duration
    // or hits floor
    // or was on floor last frame
    // if going to idle do slam land particles and slam land shake
}