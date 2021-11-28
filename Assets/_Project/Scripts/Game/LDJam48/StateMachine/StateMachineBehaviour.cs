using System;
using Unity.VisualScripting;
using UnityEngine;

namespace LDJam48.StateMachine
{
    public class StateMachineBehaviour : MonoBehaviour
    {
        [SerializeField] private StateMachine stateMachine;
        [SerializeField] private bool transitionOnUpdate = true;
        [SerializeField] private bool transitionOnFixedUpdate = true;
        public bool debugLogs = false;


        public StateRuntime PrevState;
        private StateRuntime _state;

        private void Awake()
        {
            Debug.Log($"Building state machine");
        
            Debug.Log($"Building state machine, transitions = {stateMachine.Transitions.Length}");
            _state = stateMachine.BuildRuntime(this);
            Debug.Log($"Built state machine, initial state = {_state.Name}");
        }

        private void OnEnable()
        {
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
    }
}