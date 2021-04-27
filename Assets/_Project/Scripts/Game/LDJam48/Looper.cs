using UnityEngine;

namespace LDJam48
{
    public class Looper : MonoBehaviour
    {
        [SerializeField] private float y;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                var c = other.transform.position;
                c.y = y;
                other.transform.position = c;
            }
        }
    }
}
