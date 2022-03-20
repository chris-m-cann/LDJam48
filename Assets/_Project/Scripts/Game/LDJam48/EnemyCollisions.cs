using LDJam48.PlayerState;
using Sirenix.OdinInspector;
using UnityEditor.U2D.Sprites;
using UnityEngine;
using UnityEngine.Events;
using Util;
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
                    Debug.Log("Bouncing player");
                    other.GetComponent<Bounceable>()?.Bounce();
                    Die();
                    return;
                }
            }

            // damage player
            Debug.Log("damaging player");
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
            Destroy(objectToDestroy);
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