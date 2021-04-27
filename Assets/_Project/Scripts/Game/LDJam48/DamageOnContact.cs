using System;
using UnityEngine;

namespace LDJam48
{
    public class DamageOnContact : MonoBehaviour
    {
        [SerializeField] private int amount = 1;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                other.GetComponent<IDamageable>()?.Damage(amount);
            }
        }
    }
}