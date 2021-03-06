using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using Util;
using Util.ObjPool;
using Util.Var.Events;

namespace LDJam48
{
    public class EnemyCollisions : MonoBehaviour
    {
        [SerializeField] private bool useSafeAngles;

        [ShowIf("useSafeAngles")] [SerializeField]
        private Range safeAngle;


        [SerializeField] private AudioClipAssetGameEvent sfxChannel;
        [SerializeField] private AudioClipAsset dieClip;
        [SerializeField] private string playerTag = "Player";

        [SerializeField] private ParticleEffectRequestEventReference deathEffect;

        [Tooltip("If null the current gameobject will be used")] [SerializeField]
        private GameObject objectToDestroy;

        [Tooltip("If null the current gameobject will be searched")] [SerializeField]
        private SpawnOnDeath spawner;

        [SerializeField] bool spawnOnDeath;
        [SerializeField] private LayerMask playerLayer;
        


        [SerializeField] private UnityEvent onDeath;

        [SerializeField] private bool drawGizmos;


        private void Awake()
        {
            if (spawner == null) spawner = GetComponent<SpawnOnDeath>();
            if (objectToDestroy == null) objectToDestroy = gameObject;
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

        private ContactPoint2D[] _contacts = new ContactPoint2D[4];
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!enabled || !other.CompareTag(playerTag)) return;

            bool attacking = other.GetComponent<AttackCollider>() != null;

            if (attacking)
            {
                Die();
                return;
            }
            

            if (useSafeAngles)
            {
                var relativePoint = other.gameObject.transform.position - transform.position;
                var angle = Vector2.SignedAngle(Vector2.up, relativePoint);

                var isSafe = safeAngle.Contains(angle);

                if (drawGizmos)
                {
                    Debug.DrawLine(relativePoint, transform.position, Color.red, 1f);
                }

                if (isSafe)
                {
                    // head bounce
                    other.GetComponent<Bounceable>()?.Bounce();
                    Die();
                    return;
                }
            }

            // damage player
            other.GetComponent<IDamageable>()?.Damage(1, gameObject);
        }

        public void Die()
        {
            if (deathEffect != null)
            {
                deathEffect.Raise(new ParticleEffectRequest
                {
                    Position = transform.position,
                    Rotation = transform.rotation,
                    Scale = transform.localScale
                });
            }

            if (dieClip != null)
            {
                sfxChannel.Raise(dieClip);
            }

            spawner?.SpawnObjects();
            onDeath?.Invoke();
            InstantiateEx.Destroy(objectToDestroy);
        }

        public void SetBounceable(bool isBounceable)
        {
            useSafeAngles = false;
        }

        public void SetSpawnOnDeath(bool spawnOnDeath)
        {
            this.spawnOnDeath = spawnOnDeath;
        }
    }
}