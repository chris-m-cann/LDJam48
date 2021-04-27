using UnityEngine;

namespace LDJam48
{
    public class FollowY : MonoBehaviour
    {
        [SerializeField] private Transform target;


        private void FixedUpdate()
        {
            if (target == null) return;

            var p = transform.position;

            transform.position = new Vector3(p.x, target.position.y, p.z);
        }
    }
}
