using UnityEngine;
using Util.Var;

namespace LDJam48
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class TerminalVelocity : MonoBehaviour
    {
        [SerializeField] private Vector2Reference max;

        private Rigidbody2D _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            var vel = _rb.velocity;
            vel.x = Mathf.Clamp(vel.x, -max.Value.x, max.Value.x);
            vel.y = Mathf.Clamp(vel.y, -max.Value.y, max.Value.y);

            _rb.velocity = vel;
        }
    }
}
