using UnityEngine;
using Util;

namespace LDJam48.PlayerState
{
    [CreateAssetMenu(menuName = "Custom/PlayerState/Bounce")]
    public class PlayerBounceState : PlayerState
    {
        [SerializeField] private float bounceVel = 5;
        [SerializeField] private float bounceTime = 0.1f;

        public override void OnEnter(StateMachine machine)
        {
            base.OnEnter(machine);

            machine.Context.Rigidbody2D.velocity = new Vector2(machine.Context.Rigidbody2D.velocity.x, bounceVel);

            machine.Context.ExecuteAfter(bounceTime, () =>
            {
                _machine.CurrentState = _machine.States.Falling;
            });
        }
    }
}