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


        private StateRuntime _state;

        private void Awake()
        {
            _state = stateMachine.BuildRuntime(this);
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
                _state = next;
                _state.OnStateEnter();
            }
        }
    }
}