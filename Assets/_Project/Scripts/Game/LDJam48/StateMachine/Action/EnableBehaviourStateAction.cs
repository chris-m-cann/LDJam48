using UnityEngine;

namespace LDJam48.StateMachine.Action
{
    [CreateAssetMenu(menuName = MENU_FOLDER + "/EnableBehaviour")]
    public class EnableBehaviourStateAction : StateAction
    {
        public string Behaviour;

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
            _behaviour = (MonoBehaviour)machine.GetComponent(_source.Behaviour);

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
                _behaviour.enabled = true;
            }
        }

        public override void OnStateExit()
        {
            base.OnStateExit();
            if (_behaviour != null)
            {
                _behaviour.enabled = false;
            }
        }
    }
}