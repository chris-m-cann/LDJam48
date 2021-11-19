using System;
using UnityEngine;

namespace LDJam48.StateMachine.Action
{
    [Serializable]
    public class OnCallbackStateAction : StateAction
    {
        public StateMachineCallback When = StateMachineCallback.StateEnter;
        public OneShotAction Action;

        protected override IStateAction BuildRuntimeImpl()
        {
            return new OnCallbackStateActionRuntime();
        } }

    public class OnCallbackStateActionRuntime : BaseStateActionRuntime<OnCallbackStateAction>
    {
        private IOneShotAction _sourceRuntime;
        private void RunAction()
        {
            _sourceRuntime.Execute();
        }
        
        public override void OnAwake(StateMachineBehaviour machine)
        {
            base.OnAwake(machine);
            _sourceRuntime = _source.Action.BuildRuntime();
            _sourceRuntime.OnAwake(machine);
            
            if (_source.When == StateMachineCallback.Awake)
            {
                RunAction();
            }
        }

        public override void OnStateEnter()
        {
            base.OnStateEnter();
            if (_source.When == StateMachineCallback.StateEnter)
            {
                RunAction();
            }
        }

        public override void OnStateExit()
        {
            base.OnStateExit();
            if (_source.When == StateMachineCallback.StateExit)
            {
                RunAction();
            }
        }
        
        public override void OnUpdate()
        {
            base.OnUpdate();
            if (_source.When == StateMachineCallback.Update)
            {
                RunAction();
            }
        }
        
        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
            if (_source.When == StateMachineCallback.FixedUpdate)
            {
                RunAction();
            }
        }
    }
}