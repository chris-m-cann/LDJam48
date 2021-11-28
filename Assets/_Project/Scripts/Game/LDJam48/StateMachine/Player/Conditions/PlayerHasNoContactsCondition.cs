namespace LDJam48.StateMachine.Player.Conditions
{

    [System.Serializable]
    public class PlayerHasNoContactsCondition : Condition
    {
        protected override ICondition BuildRuntimeImpl()
        {
            return new PlayerHasNoContactsConditionRuntime();
        }
    }

    public class PlayerHasNoContactsConditionRuntime : BaseConditionRuntime<PlayerHasNoContactsCondition>
    {
        private PlayerContacts _contacts;

        public override void OnAwake(StateMachineBehaviour machine)
        {
            base.OnAwake(machine);
            _contacts = machine.GetComponent<PlayerContacts>();
        }

        public override bool Evaluate()
        {
            return !_contacts.ContactDetails.IsOnFloor 
                   && !_contacts.ContactDetails.IsOnLeftWall 
                   && !_contacts.ContactDetails.IsOnRightWall;
        }
    }
}