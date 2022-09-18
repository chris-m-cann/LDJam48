using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using Util.ObjPool;
using Util.Var;

namespace LDJam48
{
    public class DamagableBehaviour : PoolableLifecycleAwareBehaviour, IDamageable
    {
        [SerializeField] protected IntReference health;
        [SerializeField] private UnityEvent onHealed;
        [SerializeField] private UnityEvent onHurt;
        [SerializeField] private UnityEvent onDead;

        private int _currentHealth;
        
        public bool IsInvincible = false;

        private void Awake()
        {
            _currentHealth = health.Value;
        }

        public virtual void Damage(int amount, GameObject damager)
        {
            if (IsInvincible || amount == 0) return;

            _currentHealth = Mathf.Max(_currentHealth - amount, 0);

            if (amount < 0)
            {
                OnHealed();
            } else if (_currentHealth == 0)
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
            onDead?.Invoke();
        }

        public override void OnPop()
        {
            base.OnPop();
            _currentHealth = health.Value;
        }
    }
}