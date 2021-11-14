using System;
using UnityEngine;
using Util;
using Util.Var;
using Random = UnityEngine.Random;

namespace LDJam48
{
    // homing scripts for projectiles that either dont care about geometry or are destroyed by it
    [RequireComponent(typeof(Rigidbody2D))]
    public class Homing : MonoBehaviour
    {
        [SerializeField] private GameObjectVariable target;
        [SerializeField] private float speed;
        [SerializeField] private float rotationalSpeed;
        [SerializeField] private Vector2 initialDelayRange;
        [SerializeField] private float lerpUpTime = 1;
        [SerializeField] private float closeDistance = 3;
        [SerializeField] private bool rightIsForward;
        
        
        

        private Rigidbody2D _rb;
        private bool _began;
        private float _beganTime;
        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void OnEnable()
        {
            this.ExecuteAfter(Random.Range(initialDelayRange.x, initialDelayRange.y), () =>
            {
                _beganTime = Time.time;
                _began = true;
            });
        }

        private void OnDisable()
        {
            _began = false;
            StopAllCoroutines();
        }

        private void FixedUpdate()
        {
            if (!_began) return;
            
            Vector2 dir = (Vector2)target.Value.transform.position - _rb.position;

            if (dir.magnitude < closeDistance)
            {
                if (rightIsForward)
                {
                    transform.right = dir.normalized;
                }
                else
                {
                    transform.up = dir.normalized;
                }
                

                _rb.velocity = dir.normalized * speed;
            }
            else
            {

                dir.Normalize();
                
                var v = rightIsForward ? transform.right : transform.up;

                float rotAmount = Vector3.Cross(dir, v).z;

                _rb.angularVelocity = -rotAmount * rotationalSpeed;

                var diff = (Time.time - _beganTime) / lerpUpTime;
                var s = Mathf.Lerp(0, speed, diff);

                _rb.velocity = v* s;
            }
        }
    }
}