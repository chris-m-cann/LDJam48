using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Unity.VisualScripting;
using UnityEngine;

namespace LDJam48.StateMachine
{
    [CreateAssetMenu(menuName = "Custom/StateMachine/Machine")]
    public class StateMachine : SerializedScriptableObject
    {
        public State InitialState;
        [OdinSerialize, NonSerialized, ListDrawerSettings(CustomAddFunction = "AddDefault")]
        public StateTransition[] Transitions = new StateTransition[0];
        
        private StateTransition AddDefault() => new StateTransition
        {
            Descriptions = new TransitionDesc[0]
        };
        
        
        public StateRuntime BuildRuntime(StateMachineBehaviour stateMachineBehaviour)
        {
            // todo check not empty
            // build a set of all state SOs
            // // instantiate a runtime instance of each state
            // build a dictionary out of the 2
            var stateSos = new HashSet<State> { InitialState };

            foreach (var transition in Transitions)
            {
                stateSos.Add(transition.From);
                foreach (var desc in transition.Descriptions)
                {
                    stateSos.Add(desc.To);
                }
            }

            var stateMappings = new Dictionary<State, StateRuntime>();
            foreach (var stateSo in stateSos)
            {
                stateMappings.Add(stateSo, stateSo.BuildRuntime());
            }
            
            // group all Transitions by from state
            var fromStates = Transitions.GroupBy(it => it.From);

            foreach (var fromState in fromStates)
            {
                // todo add null check
                var stateSo = fromState.Key;

                // convert the transitions of that state to runtime instances
                var descs = fromState.SelectMany(trans => trans.Descriptions);
                
                var transitions = descs.Select(trans =>
                {
                    // look up the runtime toState in the dictionary for the destination of each transition
                    var to = stateMappings[trans.To];
                    var conditions = trans.Conditions.Select(it => it.BuildRuntime()).ToArray();
                    var transActions = trans.OnTransitionActions.Select(it => it.BuildRuntime());

                    // always make sure the first one is an OR so that it works with the check transitions code
                    conditions[0].Operator = LogicalOperator.Or;
                    
                    return new TransitionRuntime
                    {
                        To = to,
                        Conditions = conditions,
                        OnTransitionActions = transActions.ToArray()
                    };
                }).ToArray();

                // init the runtime state with this dat
                stateMappings[stateSo].SetTransitions(transitions);
            }
            
            // propogate awake to all states
            stateMappings.Values.OnAwake(stateMachineBehaviour);
            

            return stateMappings[InitialState];
        }
    }

    public interface IStateMachineRuntimeComponent
    {
        void OnAwake(StateMachineBehaviour machine);
        void OnStateEnter();
        void OnStateExit();
    }

    public static class StateMachineRuntimeComponents
    {
        public static void OnAwake(this IEnumerable<IStateMachineRuntimeComponent> components, StateMachineBehaviour machineBehaviour)
        {
            foreach (var component in components)
            {
                component.OnAwake(machineBehaviour);
            }
        }
        public static void OnAwake(this IEnumerable<IOneShotAction> components, StateMachineBehaviour machineBehaviour)
        {
            foreach (var component in components)
            {
                component.OnAwake(machineBehaviour);
            }
        }
        
        public static void OnStateEnter(this IEnumerable<IStateMachineRuntimeComponent> components)
        {
            foreach (var component in components)
            {
                component.OnStateEnter();
            }
        }
        
        public static void OnStateExit(this IEnumerable<IStateMachineRuntimeComponent> components)
        {
            foreach (var component in components)
            {
                component.OnStateExit();
            }
        }
    }

    public enum StateMachineCallback
    {
        Awake, StateEnter, StateExit, Update, FixedUpdate
    }
}