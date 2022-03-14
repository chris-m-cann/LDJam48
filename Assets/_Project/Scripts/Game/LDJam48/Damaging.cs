using LDJam48.PlayerState;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using Util;
using Util.Var.Events;

namespace LDJam48
{
    public class Damaging : MonoBehaviour
    {
        [SerializeField] private bool useSafeAngles;

        [ShowIf("useSafeAngles")] 
        [SerializeField] private Range safeAngle;

        
        
        [SerializeField] private AudioClipAssetGameEvent sfxChannel;
        [SerializeField] private AudioClipAsset dieClip;
        [SerializeField]private string playerTag = "Player";

        [SerializeField] private ParticleEffectRequestEventReference deathEffect;

        [SerializeField] private UnityEvent onDeath;

        [SerializeField] private bool drawGizmos;
        



        private SpawnOnDeath _spawner;

        private void Awake()
        {
            _spawner = GetComponent<SpawnOnDeath>();
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

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag(playerTag)) return;

            bool attacking = other.GetComponent<AttackCollider>() != null;

            if (attacking)
            {
                Die();
                return;
            }


            var contact = other.ClosestPoint(transform.position);

            var relativePoint = contact - (Vector2) transform.position;

            Debug.DrawLine(contact, transform.position, Color.red, 1f);

            var angle = Vector2.SignedAngle(Vector2.up, relativePoint);

            var isSafe = safeAngle.Contains(angle);

            if (isSafe)
            {
                // head bounce
                other.GetComponent<Bounceable>()?.Bounce();
                Die();
            }
            else
            {
                // damage player
                other.GetComponent<IDamageable>()?.Damage(1, gameObject);
            }
        }

        private void Die()
        {

            deathEffect.Raise(new ParticleEffectRequest
            {
                Position = transform.position,
                Rotation = transform.rotation,
                Scale = transform.localScale
            });

            sfxChannel.Raise(dieClip);
            _spawner?.SpawnObjects();
            onDeath?.Invoke();
            Destroy(gameObject);
        }
    }
}