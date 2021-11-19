using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Util;

namespace LDJam48.StateMachine
{
    // todo need to make more complex so i can support &&, || and () in the conditions
    
    [Serializable]
    public struct StateTransition
    {
        public State From;
        public State To;
        public TransitionDesc Description;
    }
    
    [Serializable]
    public class TransitionDesc
    {
        public ConditionPair[] Conditions;
        [TypeFilter("GetOneShotActionTypeList")]
        public OneShotAction[] OnTransitionActions;
        
        public IEnumerable<Type> GetOneShotActionTypeList() => TypeEx.GetTypeList<OneShotAction>();
    }
    
    
    [Serializable]
    public struct TransitionRuntime
    {
        public StateRuntime To;
        public ConditionPairRuntime[] Conditions;
        public IOneShotAction[] OnTransitionActions;
    }
    
    
}