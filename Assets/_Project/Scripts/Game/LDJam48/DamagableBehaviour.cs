using UnityEngine;
using UnityEngine.Events;
using Util.Var;

namespace LDJam48
{
    public class DamagableBehaviour : MonoBehaviour, IDamageable
    {
        [SerializeField] protected IntReference health;
        [SerializeField] private UnityEvent onHealed;
        [SerializeField] private UnityEvent onHurt;
        [SerializeField] private UnityEvent onDead;
        
        
        public bool IsInvincible = false;
        public virtual void Damage(int amount, GameObject damager)
        {
            if (IsInvincible || amount == 0) return;

            health.Value = Mathf.Max(health.Value - amount, 0);

            if (amount < 0)
            {
                OnHealed();
            } else if (health.Value == 0)
            {
                OnDead();
            }
            else
            {
                OnHurt();
            }
        }       
        // this is needed to draw the enabled box in the inspector
        private void Update()
        {
        }

        public void Kill(GameObject damager)
        {
            Damage(health.Value, damager);
        }

        protected virtual void OnHealed()
        {
            onHealed?.Invoke();
        }

        protected virtual void OnHurt()
        {
            onHurt?.Invoke();
        }

        protected virtual void OnDead()
        {
            Debug.Log($"{gameObject.name} OnDead");
            onDead?.Invoke();
        }
    }
}