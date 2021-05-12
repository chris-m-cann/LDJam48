
using System;
using UnityEngine;
using Util.Var;

namespace LDJam48
{
    public class KillAfterPlayerTooFarAway : MonoBehaviour
    {
        [SerializeField] private GameObjectVariable target;

        [SerializeField] private float killDistance;
        private void Update()
        {
            if (target?.Value == null) return;

            if (transform.position.y - target.Value.transform.position.y  > killDistance)
            {
                Destroy(gameObject);
            }
        }
    }
}