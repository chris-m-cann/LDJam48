using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Util;

namespace LDJam48.StateMachine
{
    public class BounceableDamaging : MonoBehaviour
    {
        [SerializeField] private bool useSafeAngles;

        [ShowIf("useSafeAngles")] 
        [SerializeField] private Util.Range safeAngle;

        [SerializeField] private LayerMask targetLayers;
        [SerializeField] private int damage = 1;
        [Tooltip("will be set to this gameobject if left null")]
        [SerializeField] private GameObject myRoot;
        [SerializeField] private bool drawGizmos;


        private IDamageable _myDamageable;
        private void Awake()
        {
            if (myRoot == null) myRoot = gameObject;
            
            _myDamageable = myRoot.GetComponent<IDamageable>();
        }

        private void OnDrawGizmosSelected()
        {
            if (!drawGizmos || !useSafeAngles) return;

            var cache = Gizmos.color;
            Gizmos.color = Color.yellow;

            var minDir = Quaternion.Euler(0, 0, -safeAngle.Start) * Vector2.up;
            var maxDir = Quaternion.Euler(0, 0, -safeAngle.End) * Vector2.up;

            Gizmos.DrawLine(transform.position, transform.position + minDir);
            Gizmos.DrawLine(transform.position, transform.position + maxDir);

            Gizmos.color = cache;
        }
        
        // this is needed to draw the enabled box in the inspector
        private void Update()
        {
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!enabled || !targetLayers.Contains(other.gameObject.layer)) return;
            
            if (useSafeAngles)
            {
                var contact = other.ClosestPoint(transform.position);

                var relativePoint = contact - (Vector2)transform.position;
                var angle = Vector2.SignedAngle(Vector2.up, relativePoint);
                var isSafe = safeAngle.Contains(angle);

                if (drawGizmos)
                {
                    Debug.DrawLine(contact, transform.position, Color.red, 1f);
                }
                
                if (isSafe)
                {
                    // head bounce
                    Bounce(other);
                    return;
                }
            }
            
            other.GetComponent<IDamageable>()?.Damage(damage, gameObject);
        }

        private void Bounce(Collider2D other)
        {
            var bounceable = other.GetComponent<Bounceable>();
            if (bounceable == null) return;
            
            bounceable.Bounce();

            if (_myDamageable != null)
            {
                Debug.Log($"{myRoot.name}: killing the damageable");
                _myDamageable.Kill(gameObject);
            }
        }
    }
}