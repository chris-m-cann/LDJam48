using System.Collections;
using DG.Tweening;
using LDJam48.PlayerState;
using Sirenix.OdinInspector;
using UnityEngine;
using Util;
using Util.Var;
using Util.Var.Events;
using Util.Var.Observe;
using Void = Util.Void;

namespace LDJam48.StateMachine.Player.Action
{
    [System.Serializable]
    public class PlayerDashingStateAction : StateAction
    {
        public float dashSpeed = 15;
        public string anim = "player_slice";
        public string actionMap = "Dashing";
        public string onExitActionMap = "NotDashing";
        public AudioClipAsset sound;
        public ObservableStringVariable activeActionMap;
        public ShakeDefinition onDashShake;
        public ShakeDefinition onWallLandShake;
        public Vector3 trailOffset;
        public Vector3 frontTrailOffset;
        public float frontTrailDelay;
        public SpriteRenderer ghostPrefab;
        public float[] ghostDelays;


        public bool displayTrail = true;
        public VoidGameEvent dashAnimEvent;
        public LayerMask wallMask;
        [DrawWithUnity]
        public AnimationCurve dashCurve;
        public float dashSpeedMax;
        public float dashSpeedMin;
        public float leftWallX;
        public float rightWallX;
        public float boxcastSize = .5f;

        public ParticleEffectRequestEventReference onWalHit;

        public ObservableVector2Variable OnDashInput;
        public FloatVariable CarriedYVel;

        public int SoundId;
        public int LeftEffectId;
        public int RightEffectId;
        public ShakeDefinitionEventReference ShakeChannel;
        public BoolVariable WasOnWall;

        public VoidGameEvent ToOnWallEvent;
        protected override IStateAction BuildRuntimeImpl()
        {
            return new PlayerDashingStateActionRuntime();
        }
    }
    
    public class PlayerDashingStateActionRuntime : BaseStateActionRuntime<PlayerDashingStateAction>
    {

        private Vector2 _direction;
        private float _prevGravity = 1f;
        private bool _wasOnWall = false;
        private Vector2 _startPos;
        private Coroutine _coroutine;
        private Coroutine _dashCoroutine;
        private Tweener _dashTweener;
        private float _endX;
        private bool _goingToWall = false;

        private Rigidbody2D _rigidbody;
        private PlayerContacts _contacts;
        private PlayerColliders _colliders;
        private SpriteRenderer _sprite;
        private SoundsBehaviour _sounds;
        private ParticlesBehaviour _particles;
        private PlayerRaycastsBehaviour _raycasts;
        public override void OnAwake(StateMachineBehaviour machine)
        {
            base.OnAwake(machine);

            _rigidbody = machine.GetComponent<Rigidbody2D>();
            _contacts = machine.GetComponent<PlayerContacts>();
            _colliders = machine.GetComponent<PlayerColliders>();
            _sprite = machine.GetComponent<SpriteRenderer>();
            _sounds = machine.GetComponent<SoundsBehaviour>();
            _particles = machine.GetComponent<ParticlesBehaviour>();
            _raycasts = machine.GetComponent<PlayerRaycastsBehaviour>();
        }

        public override void OnStateEnter()
        {
            base.OnStateEnter();
            _direction = _source.OnDashInput.Value.normalized;

            _source.CarriedYVel.Value = _rigidbody.velocity.y;
            _prevGravity = _rigidbody.gravityScale;
            
            _rigidbody.gravityScale = 0f;
            _rigidbody.velocity = Vector2.zero;
            
            
            var isLeft = _direction.x < 0;
            _wasOnWall = _contacts.ContactDetails.IsOnLeftWall || _contacts.ContactDetails.IsOnRightWall;
            _sprite.flipX = isLeft;
            
            _source.dashAnimEvent.OnEventTrigger += StartDash;

            _colliders.Main.enabled = false;
            _colliders.Slash.enabled = true;

            // Debug.Log($"OnEnter Setting actionMap = {actionMap}");
            _source.activeActionMap.Value = _source.actionMap;
            
            
            _coroutine = _machine.StartCoroutine(ShowGhosts());
        }

        private void StartDash(Void v)
        {
            _sounds.PlaySound(_source.SoundId);

            var end = _raycasts.FindDashLandPoint(_direction, rightWallX:_source.rightWallX, leftWallX:_source.leftWallX);
            
            if (_source.WasOnWall.Value)
            {
                // its the opposite because if we are going left we wat the poof effect on the wall to our right
                var effectIdx = _direction.x < 0 ? _source.RightEffectId : _source.LeftEffectId;
                _particles.PlayEffect(effectIdx);
            }

            _dashCoroutine = _machine.StartCoroutine(CoDash(end.x));
        }

        private IEnumerator CoDash(float finalX)
        {
            var rb = _rigidbody;
            var vel = rb.velocity;

            var maxLevelWidth = _source.rightWallX - _source.leftWallX;
            var t = _machine.transform;
            var initialX = t.position.x;

            var stillGoing = true;
            while (stillGoing)
            {
                var normalised = Mathf.Abs(t.position.x - initialX) / maxLevelWidth;
                vel.x = (_source.dashSpeedMax - _source.dashSpeedMin) * _source.dashCurve.Evaluate(normalised) + _source.dashSpeedMin;
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
            vel.x = 0;
            rb.velocity = vel;

            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();
            
            _source.ToOnWallEvent.Raise();
        }

        public override void OnStateExit()
        {
            base.OnStateExit();
            _rigidbody.gravityScale = _prevGravity;
            _dashTweener?.Kill();

            if (_dashCoroutine != null)
            {
                _machine.StopCoroutine(_dashCoroutine);
            }

            _machine.StopCoroutine(_coroutine);

            _source.dashAnimEvent.OnEventTrigger -= StartDash;

            _colliders.Main.enabled = true;
            _colliders.Slash.enabled = false;

            _source.activeActionMap.Value = _source.onExitActionMap;

            _rigidbody.gravityScale = _prevGravity;
            
        }
        

        IEnumerator ShowGhosts()
        {
            foreach (var delay in _source.ghostDelays)
            {
                yield return new WaitForSeconds(delay);
                var ghost = Object.Instantiate(_source.ghostPrefab, _machine.transform.position, Quaternion.identity);
                ghost.sprite = _sprite.sprite;
                ghost.flipX = _sprite.flipX;
            }
        }
    }
    
    // on enter we play the slice anim
    // if sliding -> dash then play effects based on side and shake
    // if dash -> wall then play effects based on side and shake
}