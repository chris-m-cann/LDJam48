using System;
using Unity.VisualScripting;
using UnityEngine;
using Util;
using Util.Var;
using Util.Var.Events;

namespace LDJam48
{
    public class PlayerStateMachine : MonoBehaviour
    {
        // todo(chris) refactor this into a Context class so these arnt public to just anyone
        public InputManager InputManager;
        public ContactDetails Contacts;
        public Rigidbody2D Rigidbody2D;
        public Animator Animator;
        public SpriteRenderer Sprite;
        public AudioClipAssetGameEvent SfxChannel;
        public Collider2D MainCollider;
        public Collider2D SlashCollider;
        public Collider2D SlamCollider;

        // todo(chris) refactor these bits into individual states
        public float WallSpeed = 10;
        public float FallSpeed = 13;
        public float DashSpeed = 15;
        public float DashFallSpeed = 0;
        public float SlashTime = .2f;
        public float SlamSpeed = 10;
        public float SlamTime = .3f;
        public float BounceVel = 5;
        public float BounceTime = .1f;

        public AudioClipAsset DashClip;
        public AudioClipAsset SlashClip;
        public AudioClipAsset LandClip;
        public AudioClipAsset SlamClip;




        [SerializeField] private BoolReference isPaused;


        private StateMachine _machine;
        private PlayerContacts _playerContacts;

        private void Awake()
        {
            _playerContacts = GetComponent<PlayerContacts>();
            _machine = new StateMachine(this, new PlayerStates
            {
                Idle = new PlayerIdleState(),
                OnWall = new PlayerOnWallState(),
                Dashing = new PlayerDashingState(),
                Slashing = new PlayerSlashState(),
                Slamming = new PlayerSlamState(),
                Falling = new PlayerFallingState(),
                Bounce = new PlayerBounceState()
            }, new PlayerFallingState());
        }

        private void OnEnable()
        {
            _machine.OnStart();
            _machine.OnStateChanged += OnStateChanged;
        }

        private void OnDisable()
        {
            _machine.OnStateChanged -= OnStateChanged;
        }

        private void OnStateChanged(Pair<State, State> state)
        {
            // Debug.Log($"Changed State {state.First.Name} -> {state.Second.Name}");
        }

        private void Update()
        {
            _machine.CurrentState.OnUpdate();
        }

        private void LateUpdate()
        {
            _machine.CurrentState.OnLateUpdate();
        }

        private void FixedUpdate()
        {
            Contacts = _playerContacts.DetectContacts(Contacts);
            _machine.CurrentState.OnFixedUpdate();
        }

        public void Bounce()
        {
            _machine.CurrentState = _machine.States.Bounce;
        }
    }

    public struct PlayerStates
    {
        public PlayerIdleState Idle;
        public PlayerOnWallState OnWall;
        public PlayerDashingState Dashing;
        public PlayerSlashState Slashing;
        public PlayerSlamState Slamming;
        public PlayerFallingState Falling;
        public PlayerBounceState Bounce;
    }

    public class PlayerIdleState : State
    {
        public override void OnEnter(StateMachine machine)
        {
            base.OnEnter(machine);

            machine.Context.Animator.Play("player_idle");
            _machine.Context.SfxChannel.Raise(_machine.Context.LandClip);

            machine.Context.InputManager.OnDashLeft += OnDashLeft;
            machine.Context.InputManager.OnDashRight += OnDashRight;
        }

        public override void OnExit()
        {
            base.OnExit();

            _machine.Context.InputManager.OnDashLeft -= OnDashLeft;
            _machine.Context.InputManager.OnDashRight -= OnDashRight;
        }

        private void OnDashLeft() => _machine.CurrentState = _machine.States.Dashing.Init(Vector2.left);
        private void OnDashRight() => _machine.CurrentState = _machine.States.Dashing.Init(Vector2.right);
    }

    public class PlayerDashingState : State
    {
        private Vector2 _direction;

        public PlayerDashingState Init(Vector2 direction)
        {
            _direction = direction.normalized;
            return this;
        }

        public override void OnEnter(StateMachine machine)
        {
            base.OnEnter(machine);

            if (_direction == Vector2.left)
            {
                _machine.Context.Sprite.flipX = true;
            }
            else
            {
                _machine.Context.Sprite.flipX = false;
            }


            _machine.Context.Rigidbody2D.velocity = new Vector2(_machine.Context.DashSpeed * _direction.x, -_machine.Context.DashFallSpeed);
            _machine.Context.Animator.Play("player_dash");

            _machine.Context.SfxChannel.Raise(_machine.Context.DashClip);


            _machine.Context.InputManager.OnSlash += OnSlash;
            _machine.Context.InputManager.OnSlam += OnSlam;
        }

        public override void OnExit()
        {
            base.OnExit();


            _machine.Context.InputManager.OnSlash -= OnSlash;
            _machine.Context.InputManager.OnSlam -= OnSlam;
        }

        private void OnSlash() => _machine.CurrentState = _machine.States.Slashing;
        private void OnSlam() => _machine.CurrentState = _machine.States.Slamming;

        public override void TransitionChecks()
        {
            base.TransitionChecks();

            if (_machine.Context.Contacts.HitLeftWallThisTurn ||
                _machine.Context.Contacts.HitRightWallThisTurn)
            {
                _machine.CurrentState = _machine.CurrentState = _machine.States.OnWall;
            }
        }
    }

    public class PlayerOnWallState : State
    {
        private bool isLeft = false;
        public override void OnEnter(StateMachine machine)
        {
            base.OnEnter(machine);

            machine.Context.Rigidbody2D.velocity = new Vector2(0, -machine.Context.WallSpeed);
            machine.Context.Animator.Play("player_slide");

            isLeft = machine.Context.Contacts.IsOnLeftWall;
            machine.Context.Sprite.flipX = !isLeft;

            if (!isLeft)
            {
                machine.Context.InputManager.OnDashLeft += OnDash;
            }
            else
            {
                machine.Context.InputManager.OnDashRight += OnDash;
            }
        }

        public override void OnExit()
        {
            base.OnExit();

            if (!isLeft)
            {
                _machine.Context.InputManager.OnDashLeft -= OnDash;
            }
            else
            {
                _machine.Context.InputManager.OnDashRight -= OnDash;
            }
        }

        private void OnDash() => _machine.CurrentState = _machine.States.Dashing.Init(isLeft ? Vector2.right : Vector2.left);

        public override void TransitionChecks()
        {
            base.TransitionChecks();

            if (_machine.Context.Contacts.LeftLeftWallThisTurn || _machine.Context.Contacts.LeftRightWallThisTurn)
            {
                _machine.CurrentState = _machine.States.Falling;
            }
        }
    }


    public class PlayerSlashState : State
    {
        public override void OnEnter(StateMachine machine)
        {
            base.OnEnter(machine);

            machine.Context.Animator.Play("player_slice");

            machine.Context.MainCollider.enabled = false;
            machine.Context.SlashCollider.enabled = true;


            machine.Context.SfxChannel.Raise(machine.Context.SlashClip);

            _machine.Context.ExecuteAfter(machine.Context.SlashTime, () =>
            {
                machine.Context.MainCollider.enabled = true;
                machine.Context.SlashCollider.enabled = false;
                if (machine.CurrentState == this)
                {
                    machine.CurrentState = machine.States.Dashing; //todo(chris) add init call here to tell it which direction to go
                }
            });
        }

        public override void TransitionChecks()
        {
            base.TransitionChecks();

            if (_machine.Context.Contacts.HitFloorThisTurn)
            {
                _machine.CurrentState = _machine.CurrentState = _machine.States.Idle;
                return;
            }

            if (_machine.Context.Contacts.HitLeftWallThisTurn ||
                _machine.Context.Contacts.HitRightWallThisTurn)
            {
                _machine.CurrentState = _machine.CurrentState = _machine.States.OnWall; //todo(chris) add init call here to tell it which wall
            }
        }
    }

    public class PlayerSlamState : State
    {
        public override void OnEnter(StateMachine machine)
        {
            base.OnEnter(machine);

            machine.Context.Rigidbody2D.velocity = new Vector2(0, -machine.Context.SlamSpeed);
            machine.Context.Animator.Play("player_slam");

            machine.Context.MainCollider.enabled = false;
            machine.Context.SlamCollider.enabled = true;


            machine.Context.SfxChannel.Raise(machine.Context.SlamClip);

            _machine.Context.ExecuteAfter(machine.Context.SlamTime, () =>
            {
                machine.Context.MainCollider.enabled = true;
                machine.Context.SlamCollider.enabled = false;
                if (machine.CurrentState == this)
                {
                    machine.CurrentState = machine.States.Falling;
                }
            });
        }

        public override void TransitionChecks()
        {
            base.TransitionChecks();

            if (_machine.Context.Contacts.HitFloorThisTurn)
            {
                _machine.CurrentState = _machine.CurrentState = _machine.States.Idle;
                return;
            }
        }
    }

    public class PlayerFallingState : State
    {
        public override void OnEnter(StateMachine machine)
        {
            base.OnEnter(machine);

            if (machine.Context.Contacts.IsOnLeftWall || machine.Context.Contacts.IsOnRightWall)
            {
                machine.CurrentState = machine.States.OnWall;
                return;
            }

            machine.Context.Animator.Play("player_fall");

            machine.Context.Rigidbody2D.velocity = new Vector2(machine.Context.Rigidbody2D.velocity.x, -machine.Context.FallSpeed);

            machine.Context.InputManager.OnDashLeft += OnDashLeft;
            machine.Context.InputManager.OnDashRight += OnDashRight;
        }

        public override void OnExit()
        {
            base.OnExit();

            _machine.Context.InputManager.OnDashLeft -= OnDashLeft;
            _machine.Context.InputManager.OnDashRight -= OnDashRight;
        }

        private void OnDashLeft() => _machine.CurrentState = _machine.States.Dashing.Init(Vector2.left);
        private void OnDashRight() => _machine.CurrentState = _machine.States.Dashing.Init(Vector2.right);

        public override void TransitionChecks()
        {
            base.TransitionChecks();

            if (_machine.Context.Contacts.HitFloorThisTurn)
            {
                _machine.CurrentState = _machine.States.Idle;
                return;
            }

            if (_machine.Context.Contacts.HitLeftWallThisTurn || _machine.Context.Contacts.HitRightWallThisTurn)
            {
                _machine.CurrentState = _machine.States.OnWall;
                return;
            }
        }
    }

    public class PlayerBounceState : State
    {
        public override void OnEnter(StateMachine machine)
        {
            base.OnEnter(machine);

            machine.Context.Rigidbody2D.velocity = new Vector2(machine.Context.Rigidbody2D.velocity.x, machine.Context.BounceVel);

            machine.Context.ExecuteAfter(machine.Context.BounceTime, () =>
            {
                _machine.CurrentState = _machine.States.Falling;
            });
        }
    }

    public abstract class State
    {
        protected StateMachine _machine;

        public virtual string Name => GetType().Name;

        public virtual void OnEnter(StateMachine machine)
        {
            _machine = machine;

            TransitionChecks();
        }

        public virtual void OnExit()
        {

        }

        public virtual void TransitionChecks()
        {

        }

        public virtual void OnUpdate()
        {

        }

        public virtual void OnLateUpdate()
        {

        }

        public virtual void OnFixedUpdate()
        {
            TransitionChecks();
        }
    }



    public class StateMachine
    {
        public event Action<Pair<State, State>> OnStateChanged;

        public PlayerStateMachine Context;
        public PlayerStates States;

        private State _currentState;
        public State CurrentState
        {
            get => _currentState;
            set
            {
                if (_currentState != value)
                {
                    var oldState = _currentState;

                    _currentState.OnExit();

                    _currentState = value;

                    OnStateChanged?.Invoke(PairEx.Make(oldState, _currentState));

                    _currentState.OnEnter(this);
                }
            }
        }

        public StateMachine(
            PlayerStateMachine context,
            PlayerStates states,
            State initialState
            )
        {
            Context = context;
            States = states;
            _currentState = initialState;
        }

        public void OnStart()
        {
            _currentState.OnEnter(this);
        }
    }
}