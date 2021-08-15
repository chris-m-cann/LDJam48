using UnityEngine;

namespace LDJam48
{
    public class Follow : MonoBehaviour
    {
        [SerializeField] private string targetTag;
        [SerializeField] private bool followX = true;
        [SerializeField] private bool followY = true;
        


        private Transform _target;

        private void Start()
        {
            _target = GameObject.FindWithTag(targetTag)?.transform;
        }

        private void FixedUpdate()
        {
            if (_target == null) return;

            var p = transform.position;

            var x = p.x;
            if (followX)
            {
                x = _target.position.x;
            }

            var y = p.y;
            if (followY)
            {
                y = _target.position.y;
            }

            transform.position = new Vector3(x, y, p.z);
        }
    }
}
