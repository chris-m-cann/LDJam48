using System;
using Util;

namespace LDJam48.PlayerState
{
    public class StateMachine
    {
        public event Action<Pair<PlayerState, PlayerState>> OnStateChanged;

        public PlayerStateMachine Context;
        public PlayerStates States;

        private PlayerState _currentState;
        public PlayerState CurrentState
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
            PlayerState initialState
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

        public void OnDestroy()
        {
            _currentState.OnExit();
        }
    }
}