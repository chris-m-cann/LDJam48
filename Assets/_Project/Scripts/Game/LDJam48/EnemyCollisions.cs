using UnityEngine;
using Util;
using Util.Var.Events;

namespace LDJam48
{
    public class EnemyCollisions : MonoBehaviour
    {
        [SerializeField] private Bounds[] safeColliders;


        [SerializeField] private AudioClipAssetGameEvent sfxChannel;
        [SerializeField] private AudioClipAsset dieClip;

        [SerializeField]private string playerTag = "Player";


        private SpawnOnDeath _spawner;

        private void Awake()
        {
            _spawner = GetComponent<SpawnOnDeath>();
        }

        private void OnDrawGizmosSelected()
        {
            if (safeColliders.Length == 0) return;

            var cache = Gizmos.color;
            Gizmos.color = Color.yellow;
            foreach (var safeCollider in safeColliders)
            {
                Gizmos.DrawWireCube(transform.position + safeCollider.center, safeCollider.extents);
            }

            Gizmos.DrawWireSphere(transform.position, .1f);

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
            var isSafe = false;

            var relativePoint = contact - (Vector2) transform.position;

            Debug.DrawLine(contact, transform.position, Color.red, 1f);


            foreach (var safeCollider in safeColliders)
            {
                if (safeCollider.Contains(relativePoint))
                {
                    isSafe = true;
                    break;
                }
            }


            if (isSafe)
            {
                // head bounce
                other.GetComponent<PlayerController>()?.Bounce();
                Die();
            }
            else
            {
                // damage player
                other.GetComponent<IDamageable>()?.Damage(1);
            }
        }

        private void Die()
        {
            sfxChannel.Raise(dieClip);
            _spawner.SpawnObjects();
            Destroy(gameObject);
        }
    }
}