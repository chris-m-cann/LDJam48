
using UnityEngine;

namespace LDJam48.StateMachine.Player.Conditions
{
    [System.Serializable]
    public class PlayerIsOnWallCondition : Condition
    {
        protected override ICondition BuildRuntimeImpl()
        {
            return new PlayerIsOnWallConditionRuntime();
        }
    }
    
    public class PlayerIsOnWallConditionRuntime : BaseConditionRuntime<PlayerIsOnWallCondition>
    {
        private PlayerContacts _contacts;

        public override void OnAwake(StateMachineBehaviour machine)
        {
            base.OnAwake(machine);
            _contacts = machine.GetComponent<PlayerContacts>();
        }

        public override bool Evaluate()
        {
            var r = _contacts.ContactDetails.IsOnLeftWall || _contacts.ContactDetails.IsOnRightWall;

            return r;
        }
    }
}