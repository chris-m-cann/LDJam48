using UnityEngine;

namespace LDJam48
{
    public interface IDamageable
    {
        void Damage(int amount, GameObject damager);
        void Kill(GameObject damager);
    }
}