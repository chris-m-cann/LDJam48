using System;
using System.Linq;
using UnityEngine;

namespace LDJam48.StateMachine
{
    [CreateAssetMenu(menuName = "Custom/StateMachine/State")]
    public class State : ScriptableObject
    {
        public StateAction[] Actions;

        public IStateAction[] BuildActions()
        {
            return Actions.Select(it => it.BuildRuntime()).ToArray();
        }
    }

    public class StateRuntime : IStateMachineRuntimeComponent
    {
        public string Name;
        private IStateAction[] _actions;
        private TransitionRuntime[] _transitions = Array.Empty<TransitionRuntime>();


        public StateRuntime(string name, IStateAction[] actions)
        {
            Name = name;
            _actions = actions ?? Array.Empty<IStateAction>();
        }

        public void SetTransitions(TransitionRuntime[] transitions)
        {
            _transitions = transitions ?? Array.Empty<TransitionRuntime>();

            for (var i = 0; i < _transitions.Length; i++)
            {
                _transitions[i].OnTransitionActions ??= Array.Empty<IOneShotAction>();
            }
        }

        public void OnAwake(StateMachineBehaviour machine)
        {
            _actions.OnAwake(machine);

            foreach (var transition in _transitions)
            {
                transition.Conditions.OnAwake(machine);
                transition.OnTransitionActions.OnAwake(machine);
            }
        }

        public void OnStateEnter()
        {
            _actions.OnStateEnter();

            foreach (var transition in _transitions)
            {
                transition.Conditions.OnStateEnter();
            }
        }

        public void OnStateExit()
        {
            _actions.OnStateExit();

            foreach (var transition in _transitions)
            {
                transition.Conditions.OnStateExit();
            }
        }

        public void OnUpdate()
        {
            foreach (var action in _actions)
            {
                action.OnUpdate();
            }
        }

        public void OnFixedUpdate()
        {
            foreach (var action in _actions)
            {
                action.OnFixedUpdate();
            }
        }

        public StateRuntime CheckTransitions()
        {
            foreach (var transition in _transitions)
            {
                bool doTransition = false;
                
                foreach (var condition in transition.Conditions)
                {
                    var r = condition.Condition.Evaluate();
                    switch (condition.Operator)
                    {
                        case LogicalOperator.And:
                            doTransition = doTransition && r;
                            break;
                        case LogicalOperator.Or:
                            doTransition = doTransition || r;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(condition.Operator));
                    }
                }

                if (doTransition)
                {
                    foreach (var action in transition.OnTransitionActions)
                    {
                        action.Execute();
                    }

                    return transition.To;
                }
            }
            
            return null;
        }
    }
}