using System;
using UnityEngine;
using Util;

namespace LDJam48.StateMachine.Action
{
    [Serializable]
    public class EnableBehaviourStateAction : StateAction
    {
        public string Behaviour;
        [Tooltip("on enter Behaviour.enabled = enabledOnEnter")]
        public bool enabledOnEnter = true;
        [Tooltip("on exit Behaviour.enabled = enabledOnExit")]
        public bool enabledOnExit = false;

        public bool checkInChildren;
        protected override IStateAction BuildRuntimeImpl()
        {
            return new EnableBehaviourStateActionRuntime();
        }
    }

    public class EnableBehaviourStateActionRuntime : BaseStateActionRuntime<EnableBehaviourStateAction>
    {
        private MonoBehaviour _behaviour;

        public override void OnAwake(StateMachineBehaviour machine)
        {
            base.OnAwake(machine);
            if (_source.checkInChildren)
            {
                _behaviour = (MonoBehaviour)machine.GetComponentInChildren(_source.Behaviour);
            }
            else
            {
                _behaviour = (MonoBehaviour)machine.GetComponent(_source.Behaviour);
            }

            if (_behaviour == null)
            {
                Debug.LogError($"{_machine.name} : {Name} : behaviour {_source.Behaviour} not found");
            }
        }

        public override void OnStateEnter()
        {
            base.OnStateEnter();
            if (_behaviour != null)
            {
                _behaviour.enabled = _source.enabledOnEnter;
            }
        }

        public override void OnStateExit()
        {
            base.OnStateExit();
            if (_behaviour != null)
            {
                _behaviour.enabled = _source.enabledOnExit;
            }
        }
    }
}