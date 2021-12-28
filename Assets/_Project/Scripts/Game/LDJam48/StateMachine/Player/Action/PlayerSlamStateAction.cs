using System;
using System.Collections;
using Sirenix.OdinInspector;
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

        [DrawWithUnity] public AnimationCurve speedCurve;
        public float maxSpeed;
        public float minSpeed;
        public float maxDistance;

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
        private PlayerRaycastsBehaviour _raycasts;

        private const int EXIT_CODE_FALLING = 0;
        private const int EXIT_CODE_IDLE = 0;

        public override void OnAwake(StateMachineBehaviour machine)
        {
            base.OnAwake(machine);

            _rigidbody = machine.GetComponent<Rigidbody2D>();
            _colliders = machine.GetComponent<PlayerColliders>();
            _particles = machine.GetComponent<ParticlesBehaviour>();
            _sounds = machine.GetComponent<SoundsBehaviour>();
            _raycasts = machine.GetComponent<PlayerRaycastsBehaviour>();
        }

        public override void OnStateEnter()
        {
            base.OnStateEnter();

            _source.maxVelocity.Value = new Vector2(_source.maxVelocity.Value.x, _source.maxSpeed);
            _rigidbody.velocity = new Vector2(0, -_source.maxSpeed);

            _colliders.Main.enabled = false;
            _colliders.Slam.enabled = true;

            _particles.PlayEffect(_source.SlamEffectId);
            _particles.PlayEffect(_source.OngoingSlamEffectId);

            _sounds.PlaySound(_source.SlamSoundId);

            var finalPos = _raycasts.FindSlamLandPoint(_source.maxDistance);
            _machine.StartCoroutine(CoSlam(finalPos.y));
        }


        public override void OnStateExit()
        {
            base.OnStateExit();
            _machine.StopAllCoroutines();
            _colliders.Slam.enabled = false;
            _colliders.Main.enabled = true;
            _particles.StopEffect(_source.OngoingSlamEffectId);
            
            _rigidbody.velocity = new Vector2(0, _source.CarriedYVel.Value);
        }

        private IEnumerator CoSlam(float finalY)
        {
            var vel = _rigidbody.velocity;
            float startY = _machine.transform.position.y;
            float minYVel = Mathf.Max(_source.minSpeed, _source.CarriedYVel.Value);

            while (_machine.transform.position.y > finalY)
            {
                float distanceTravelled = startY - _machine.transform.position.y;
                var normalised = distanceTravelled / _source.maxDistance;
                vel.y = (_source.maxSpeed - minYVel) * _source.speedCurve.Evaluate(normalised) + minYVel;
                vel.y *= -1;

                _rigidbody.velocity = vel;

                yield return null;
            }

            var pos = _machine.transform.position;
            pos.y = finalY;
            _machine.transform.position = pos;
            
            yield return new WaitForFixedUpdate();

            bool isInAir = Mathf.Abs((startY - finalY) - _source.maxDistance) < .5f;
            int exitCode = isInAir ? EXIT_CODE_FALLING : EXIT_CODE_IDLE;
            _machine.StateComplete(exitCode);
        }
    }

    // on enter anim
    // transition out after duration
    // or hits floor
    // or was on floor last frame
    // if going to idle do slam land particles and slam land shake
}