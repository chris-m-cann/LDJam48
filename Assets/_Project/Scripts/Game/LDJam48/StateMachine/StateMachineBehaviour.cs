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
        

        private StateRuntime _state;

        private void Awake()
        {
            _state = stateMachine.BuildRuntime(this);
        }

        private void OnEnable()
        {
            _state.OnStateEnter();
        }

        private void OnDisable()
        {
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
                _state.OnStateExit();
                _state = next;
                _state.OnStateEnter();
            }
        }
    }
}