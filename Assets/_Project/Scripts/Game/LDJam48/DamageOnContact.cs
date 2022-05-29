using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using Util;

namespace LDJam48
{
    public class DamageOnContact : MonoBehaviour
    {
        [SerializeField] private int amount = 1;

        [HideIf("$isLayerBased"), SerializeField]
        private string tagToDamage = "Player";

        [SerializeField] private bool isLayerBased;

        [ShowIf("$isLayerBased"), SerializeField]
        private LayerMask layersToDamage;

        // this is needed to draw the enabled box in the inspector
        private void Update()
        {
            
        }

        [SerializeField] private UnityEvent afterCollision;
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!isActiveAndEnabled) return;
            if (isLayerBased)
            {
                if (layersToDamage.Contains(other.gameObject.layer))
                {
                    other.GetComponent<IDamageable>()?.Damage(amount, gameObject);
                }
            }
            else
            {
                if (other.CompareTag(tagToDamage))
                {
                    other.GetComponent<IDamageable>()?.Damage(amount, gameObject);
                }
            }
            
            afterCollision?.Invoke();
        }
    }
}