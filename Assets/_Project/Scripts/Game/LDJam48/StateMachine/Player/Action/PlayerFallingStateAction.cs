using UnityEngine;
using Util.Var;

namespace LDJam48.StateMachine.Player.Action
{
    [System.Serializable]
    public class PlayerFallingStateAction : StateAction
    {
        public float fallSpeed = 13;
        public Vector2Reference maxVelocity;
        public FloatVariable CarriedYVel;

        protected override IStateAction BuildRuntimeImpl()
        {
            return new PlayerFallingStateActionRuntime();
        }
    }
    
    public class PlayerFallingStateActionRuntime : BaseStateActionRuntime<PlayerFallingStateAction>
    {
        public override void OnStateEnter()
        {
            base.OnStateEnter();
            
            Debug.Log("Entering Falling state Action");

            _source.maxVelocity.Value = new Vector2(_source.maxVelocity.Value.x, _source.fallSpeed);
        }

        public override void OnStateExit()
        {
            base.OnStateExit();
            
            
            Debug.Log("Entering Exiting falling state action");
            
            _source.CarriedYVel.Value = _machine.GetComponent<Rigidbody2D>().velocity.y;
        }
        
    }
    
    // on enter set anim
    // if falling -> idle : particles, shake
}