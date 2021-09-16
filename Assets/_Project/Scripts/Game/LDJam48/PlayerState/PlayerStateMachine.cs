using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Util;
using Util.Var;
using Util.Var.Events;

namespace LDJam48.PlayerState
{
    public class PlayerStateMachine : MonoBehaviour
    {
        [SerializeField] private PlayerStates states;


        // todo(chris) refactor this into a Context class so these arnt public to just anyone
        // also needs splitting into state specific stuff?? move states to components maybe?
        public ContactDetails Contacts;
        public Rigidbody2D Rigidbody2D;
        public Animator Animator;
        public SpriteRenderer Sprite;
        public AudioClipAssetGameEvent SfxChannel;
        public Collider2D MainCollider;
        public Collider2D SlashCollider;
        public Collider2D SlamCollider;
        public float CarriedYVel;
        public Transform LeftEffectPoint;
        public Transform RightEffectPoint;
        public Transform BottomEffectPoint;
        public Transform SlamEffectPoint;
        public ParticleEffectRequestEventReference DashEffectEvent;
        public ParticleEffectRequestEventReference WallImpactEffectEvent;
        public ParticleEffectRequestEventReference FloorImpactEffectEvent;
        public ParticleSystem WallSlideParticles;
        public ShakeDefinitionEventReference ShakeEvent;

        public Transform LeftProjectionPoint;
        public Transform RightProjectionPoint;

        public ParticleSystem DashTrailEffect;
        public ParticleSystem SlamParticles;

        public event Action<Vector2> OnDashInput;
        public event Action OnSlamInput;


      [SerializeField] private BoolReference isPaused;

      [SerializeField] private string activeState;
      [SerializeField] private bool logStateChanges;
      



        private StateMachine _machine;
        private PlayerContacts _playerContacts;

        private void Awake()
        {
            _playerContacts = GetComponent<PlayerContacts>();
            _machine = new StateMachine(this, states, states.Falling);
        }

        private void OnEnable()
        {
            _machine.OnStart();
            _machine.OnStateChanged += OnStateChanged;
        }

        private void OnDisable()
        {
            _machine.OnDestroy();
            _machine.OnStateChanged -= OnStateChanged;
        }

        private void OnStateChanged(Pair<PlayerState, PlayerState> state)
        {
            if (logStateChanges)
            {
                Debug.Log($"Player State change {state.First.Name} -> {state.Second.Name}");
            }

            activeState = state.Second.Name;
        }

        private void Update()
        {
            if (isPaused.Value) return;

            _machine.OnUpdate();
        }

        private void LateUpdate()
        {
            if (isPaused.Value) return;
            _machine.OnLateUpdate();
        }

        private void FixedUpdate()
        {
            if (isPaused.Value) return;
            Contacts = _playerContacts.DetectContacts(Contacts);
            _machine.OnFixedUpdate();
        }

        public void Bounce()
        {
            _machine.CurrentState = _machine.States.Bounce;
        }
        

        public void Dash(Vector2 direction) => OnDashInput?.Invoke(direction);

        public void Slam()
        {
            OnSlamInput?.Invoke();
        }
    }

    [Serializable]
    public struct PlayerStates
    {
        public PlayerIdleState Idle;
        public PlayerOnWallState OnWall;
        public PlayerDashingState Dashing;
        public PlayerSpecialState Slamming;
        public PlayerFallingState Falling;
        public PlayerBounceState Bounce;
    }

}
