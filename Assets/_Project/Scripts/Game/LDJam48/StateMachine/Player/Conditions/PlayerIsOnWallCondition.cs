
using UnityEngine;
using Util;

namespace LDJam48.StateMachine.Player.Conditions
{
    [System.Serializable]
    public class PlayerIsOnWallCondition : Condition
    {
        public LayerMask WallLayers;
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
            var r = false;
            if (_contacts.ContactDetails.IsOnLeftWall)
            {
                r = _source.WallLayers.Contains(_contacts.ContactDetails.LeftWallCollider.gameObject.layer);
            } else if (_contacts.ContactDetails.IsOnRightWall)
            {
                r = _source.WallLayers.Contains(_contacts.ContactDetails.RightWallCollider.gameObject.layer);
            }

            return r;
        }
    }
}