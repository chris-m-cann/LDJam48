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
        [ListDrawerSettings(CustomAddFunction = "AddDefault")]
        public TransitionDesc[] Descriptions;

        private TransitionDesc AddDefault() => new TransitionDesc
        {
            Conditions = new ConditionPair[0],
            OnTransitionActions = new OneShotAction[0]
        };
    }
    
    [Serializable]
    public struct TransitionDesc
    {
        public State To;
        public ConditionPair[] Conditions;

        [TypeFilter("GetOneShotActionTypeList")]
        public OneShotAction[] OnTransitionActions;
        
   
        private TransitionDesc AddDefault() => new TransitionDesc();
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