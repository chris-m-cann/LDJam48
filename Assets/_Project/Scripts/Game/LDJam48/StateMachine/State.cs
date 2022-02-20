using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sirenix.OdinInspector;
using UnityEngine;
using Util;

namespace LDJam48.StateMachine
{
    [CreateAssetMenu(menuName = "Custom/StateMachine/State")]
    public class State : SerializedScriptableObject
    {
        [TypeFilter("GetActionTypeList")]
        public StateAction[] Actions = new StateAction[0];
        [TypeFilter("GetOneShotActionTypeList")]
        public OneShotAction[] OnEnterActions = new OneShotAction[0];
        [TypeFilter("GetOneShotActionTypeList")]
        public OneShotAction[] OnExitActions = new OneShotAction[0];
        [TypeFilter("GetOneShotActionTypeList")]
        public OneShotAction[] OnUpdateActions = new OneShotAction[0];
        [TypeFilter("GetOneShotActionTypeList")]
        public OneShotAction[] OnFixedUpdateActions = new OneShotAction[0];

        public bool LogTrueConditions;
        public IEnumerable<Type> GetActionTypeList() => TypeEx.GetTypeList<StateAction>();

        public IEnumerable<Type> GetOneShotActionTypeList() => TypeEx.GetTypeList<OneShotAction>();

        public StateRuntime BuildRuntime()
        {
            return new StateRuntime(
                this,
                Actions.Select(it => it.BuildRuntime()).ToArray(),
                OnEnterActions.Select(it => it.BuildRuntime()).ToArray(),
                OnExitActions.Select(it => it.BuildRuntime()).ToArray(),
                OnUpdateActions.Select(it => it.BuildRuntime()).ToArray(),
                OnFixedUpdateActions.Select(it => it.BuildRuntime()).ToArray()
            );
        }
    }

    public class StateRuntime : IStateMachineRuntimeComponent
    {
        public string Name => _source.name;
        private readonly State _source;
        private readonly IStateAction[] _actions;
        private readonly IOneShotAction[] _onEnterActions;
        private readonly IOneShotAction[] _onExitActions;
        private readonly IOneShotAction[] _onUpdateActions;
        private readonly IOneShotAction[] _onFixedUpdateActions;
        private TransitionRuntime[] _transitions = Array.Empty<TransitionRuntime>();
        private StateMachineBehaviour _machine;


        public StateRuntime(
            State source,
            IStateAction[] actions,
            IOneShotAction[] onEnterActions,
            IOneShotAction[] onExitActions,
            IOneShotAction[] onUpdateActions,
            IOneShotAction[] onFixedUpdateActions
        )
        {
            _source = source;
            _actions = actions ?? Array.Empty<IStateAction>();
            _onEnterActions = onEnterActions ?? Array.Empty<IOneShotAction>();
            _onExitActions = onExitActions ?? Array.Empty<IOneShotAction>();
            _onUpdateActions = onUpdateActions ?? Array.Empty<IOneShotAction>();
            _onFixedUpdateActions = onFixedUpdateActions ?? Array.Empty<IOneShotAction>();
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
            _machine = machine;
            _actions.OnAwake(machine);
            _onEnterActions.OnAwake(machine);
            _onExitActions.OnAwake(machine);
            _onUpdateActions.OnAwake(machine);
            _onFixedUpdateActions.OnAwake(machine);

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

            foreach (var action in _onEnterActions)
            {
                action.Execute();
            }
        }

        public void OnStateExit()
        {
            _actions.OnStateExit();

            foreach (var transition in _transitions)
            {
                transition.Conditions.OnStateExit();
            }
            
            foreach (var action in _onExitActions)
            {
                action.Execute();
            }
        }

        public void OnUpdate()
        {
            foreach (var action in _actions)
            {
                action.OnUpdate();
            }
            
            foreach (var action in _onUpdateActions)
            {
                action.Execute();
            }
        }

        public void OnFixedUpdate()
        {
            foreach (var action in _actions)
            {
                action.OnFixedUpdate();
            }
            
            foreach (var action in _onFixedUpdateActions)
            {
                action.Execute();
            }
        }

        private Pair<string, bool>[] results = new Pair<string, bool>[10];

        public StateRuntime CheckTransitions()
        {
            for (var i = 0; i < _transitions.Length; i++)
            {
                var transition = _transitions[i];
                bool doTransition = false;

                for (var j = 0; j < transition.Conditions.Length; j++)
                {
                    var condition = transition.Conditions[j];
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

                    results[j] = new Pair<string, bool>(condition.Condition.Name, r);
                }

                if (doTransition)
                {
                    if (_machine.debugLogs)
                    {
                        var sb = new StringBuilder();
                        for (var j = 0; j < transition.Conditions.Length; j++)
                        {
                            var result = results[j];
                            sb.Append($"{result.First}={result.Second}, ");
                        }

                        Debug.Log($"{Name}->{transition.To.Name}. because: {sb}");
                    }


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