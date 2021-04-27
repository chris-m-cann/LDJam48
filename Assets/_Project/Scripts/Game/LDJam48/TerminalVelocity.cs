using UnityEngine;

namespace LDJam48
{
    public class TerminalVelocity : MonoBehaviour
    {
        [SerializeField] private Vector2 max;

        private Rigidbody2D _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            var vel = _rb.velocity;
            vel.x = Mathf.Clamp(vel.x, -max.x, max.x);
            vel.y = Mathf.Clamp(vel.y, -max.y, max.y);

            _rb.velocity = vel;
        }
    }
}
