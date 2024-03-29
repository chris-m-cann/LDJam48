using System;
using System.Diagnostics.CodeAnalysis;
using Unity.VisualScripting;
using UnityEngine;
using Util.Var.Events;

namespace LDJam48.StateMachine
{
    public class StateMachineBehaviour : MonoBehaviour
    {
        [SerializeField] private StateMachine stateMachine;
        [SerializeField] private bool transitionOnUpdate = true;
        [SerializeField] private bool transitionOnFixedUpdate = true;
        public event Action<int> StateCompletedItself;
        
        public bool debugLogs = false;


        public StateRuntime PrevState;
        private StateRuntime _state;
        private StateRuntime _initialState;

        private StateRuntime _nullState = new StateRuntime(
            null,
            new IStateAction[] { },
            new IOneShotAction[] { },
            new IOneShotAction[] { },
            new IOneShotAction[] { },
            new IOneShotAction[] { }
        );

        private void Awake()
        {
            _initialState = stateMachine.BuildRuntime(this);
        }

        private void OnEnable()
        {
            _state = _initialState;
            if (debugLogs)
            {
                Debug.Log($"{name}: entering initial state: {_state.Name}");
            }

            _state.OnStateEnter();
        }

        private void OnDisable()
        {
            if (debugLogs)
            {
                Debug.Log($"{name}: exit state: {_state.Name}");
            }

            _state.OnStateExit();
        }

        private void Update()
        {
            if (transitionOnUpdate)
            {
                CheckTransitions();
            }

            _state.OnUpdate();
        }

        private void FixedUpdate()
        {
            if (transitionOnFixedUpdate)
            {
                CheckTransitions();
            }

            _state.OnFixedUpdate();
        }
        

        private void CheckTransitions()
        {
            var next = _state.CheckTransitions();
            if (next != null)
            {
                if (debugLogs)
                {
                    Debug.Log($"{name}: state: {_state.Name} -> {next.Name}");
                }

                _state.OnStateExit();
                PrevState = _state;
                _state = next;
                _state.OnStateEnter();
            }
        }

        public void StateComplete(int exitCode)
        {
            StateCompletedItself?.Invoke(exitCode);
        }

        public void Stop()
        {
            _state = _nullState;
        }

    }
}