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
        public ContactDetails Contacts;
        public Rigidbody2D Rigidbody2D;
        public Animator Animator;
        public SpriteRenderer Sprite;
        public AudioClipAssetGameEvent SfxChannel;
        public Collider2D MainCollider;
        public Collider2D SlashCollider;
        public Collider2D SlamCollider;

        public event Action<Vector2> OnDashInput;
        public event Action OnSlamInput;


      [SerializeField] private BoolReference isPaused;


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

        private void OnStateChanged(Pair<LDJam48.PlayerState.PlayerState, LDJam48.PlayerState.PlayerState> state)
        {
            // Debug.Log($"Changed State {state.First.Name} -> {state.Second.Name}");
        }

        private void Update()
        {
            if (isPaused.Value) return;

            _machine.CurrentState.OnUpdate();
        }

        private void LateUpdate()
        {
            if (isPaused.Value) return;
            _machine.CurrentState.OnLateUpdate();
        }

        private void FixedUpdate()
        {
            if (isPaused.Value) return;
            Contacts = _playerContacts.DetectContacts(Contacts);
            _machine.CurrentState.OnFixedUpdate();
        }

        public void Bounce()
        {
            _machine.CurrentState = _machine.States.Bounce;
        }

        public void Dash(Vector2 direction) => OnDashInput?.Invoke(direction);

        public void Slam()
        {
            Debug.Log("Slam done called");
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
