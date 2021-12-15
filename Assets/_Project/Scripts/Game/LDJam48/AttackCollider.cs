using System;
using UnityEngine;

namespace LDJam48
{
    [RequireComponent(typeof(Collider2D))]
    public class AttackCollider : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!isActiveAndEnabled) return;
            Debug.Log($"{name} collided with {other.name}");
        }
    }
}