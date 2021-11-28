
using UnityEngine;

namespace LDJam48.StateMachine.Player.Conditions
{
    [System.Serializable]
    public class PlayerIsOnFloorCondition : Condition
    {
        protected override ICondition BuildRuntimeImpl()
        {
            return new PlayerIsOnFloorConditionRuntime();
        }
    }
    
    public class PlayerIsOnFloorConditionRuntime : BaseConditionRuntime<PlayerIsOnFloorCondition>
    {
        private PlayerContacts _contacts;

        private float _nextT = 0;

        public override void OnAwake(StateMachineBehaviour machine)
        {
            base.OnAwake(machine);
            _contacts = machine.GetComponent<PlayerContacts>();
        }

        public override bool Evaluate()
        {
            var ponf = _contacts.ContactDetails.IsOnFloor;

            if (Time.time > _nextT)
            {
                Debug.Log($"Player on Floor = {ponf}");
                _nextT = Time.time + 500;
            }


            return ponf;
        }
    }
}