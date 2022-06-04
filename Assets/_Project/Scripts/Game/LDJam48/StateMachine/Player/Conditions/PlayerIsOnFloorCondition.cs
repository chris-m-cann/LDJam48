
using UnityEngine;
using Util;

namespace LDJam48.StateMachine.Player.Conditions
{
    [System.Serializable]
    public class PlayerIsOnFloorCondition : Condition
    {
        public LayerMask floorLayers;
        public bool invert = false;
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
            var ponf = _contacts.ContactDetails.IsOnFloor && _source.floorLayers.Contains(_contacts.ContactDetails.FloorCollider.gameObject.layer);

            if (Time.time > _nextT)
            {
                _nextT = Time.time + 500;
            }


            return _source.invert ? !ponf : ponf;
        }
    }
}