
using System;
using UnityEngine;

namespace LDJam48
{
    public class KillAfterPlayerTooFarAway : MonoBehaviour
    {
        [SerializeField] private Transform target;

        [SerializeField] private float killDistance;


        private void Start()
        {
            target = FindObjectOfType<PlayerController>()?.transform;
        }

        private void Update()
        {
            if (target == null) return;

            if (transform.position.y - target.position.y  > killDistance)
            {
                Destroy(gameObject);
            }
        }
    }
}