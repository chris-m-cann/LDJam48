using System;

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
    public struct TransitionDesc
    {
        public ConditionPair[] Conditions;
        public OneShotAction[] OnTransitionActions;
    }
    
    
    [Serializable]
    public struct TransitionRuntime
    {
        public StateRuntime To;
        public ConditionPairRuntime[] Conditions;
        public IOneShotAction[] OnTransitionActions;
    }
    
    
}