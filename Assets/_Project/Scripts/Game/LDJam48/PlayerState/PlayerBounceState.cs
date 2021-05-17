using UnityEngine;
using Util;

namespace LDJam48.PlayerState
{
    [CreateAssetMenu(menuName = "Custom/PlayerState/Bounce")]
    public class PlayerBounceState : PlayerState
    {
        [SerializeField] private float bounceVel = 5;
        [SerializeField] private float bounceTime = 0.1f;

        public override PlayerState OnEnter()
        {
            _machine.Context.Rigidbody2D.velocity = new Vector2(_machine.Context.Rigidbody2D.velocity.x, bounceVel);

            _machine.Context.ExecuteAfter(bounceTime, () =>
            {
                _machine.CurrentState = _machine.States.Falling;
            });

            return null;
        }
    }
}