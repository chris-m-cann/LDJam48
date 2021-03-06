using System;
using UnityEngine;
using UnityEngine.Playables;
using Util;

namespace LDJam48.PlayerState
{
    public class StateMachine
    {
        public event Action<Pair<PlayerState, PlayerState>> OnStateChanged;

        public PlayerStateMachine Context;
        public PlayerStates States;

        private PlayerState _currentState;
        private PlayerState _nullState;

        public PlayerState CurrentState
        {
            get => _currentState;
            set
            {
                if (value == null) return ;
                if (value == _currentState) return;

                var prev = _currentState;

                // set up a null state in case out of phase updates come in while we are switching
                // eg if this is called in update and the fixed update runs
                _currentState = _nullState;

                _currentState = SetState(prev, value);

                prev.OnExit(_currentState);
                _currentState.OnEnter(prev);
            }
        }

        private PlayerState SetState(PlayerState prev, PlayerState next)
        {
            if (next == null) return prev;
            if (next == prev) return prev;

            next.SetMachine(this);

            OnStateChanged?.Invoke(PairEx.Make(prev, next));

            // if initial checks for the state change us then dont bother starting that state
            var checks = next.TransitionChecks();
            if (checks != null && checks != next)
            {
                return SetState(next, checks);
            }

            // made it passed initialisation and entering so we are the final choice!!
            return next;
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
            _currentState.SetMachine(this);

            _nullState = ScriptableObject.CreateInstance<NullPlayerState>();
        }

        public void OnStart()
        {
            _currentState.OnEnter(_currentState);
        }

        public void OnDestroy()
        {
            _currentState.OnExit(_currentState);
        }

        public void OnUpdate()
        {
            CurrentState = _currentState.OnUpdate();
        }


        public void OnLateUpdate()
        {
            CurrentState = _currentState.OnLateUpdate();
        }

        public void OnFixedUpdate()
        {
            CurrentState = _currentState.OnFixedUpdate();
        }
    }

    class NullPlayerState : PlayerState
    {

    }
}