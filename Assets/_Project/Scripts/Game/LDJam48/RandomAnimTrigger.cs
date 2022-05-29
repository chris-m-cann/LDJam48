using System;
using System.Collections;
using UnityEngine;
using Util;
using Random = UnityEngine.Random;

namespace LDJam48
{
    [RequireComponent(typeof(Animator))]
    public class RandomAnimTrigger : MonoBehaviour
    {
        [SerializeField] private string[] triggers;
        [SerializeField] private float minWait;
        [SerializeField] private float maxWait;

        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private IEnumerator Start()
        {
            while (isActiveAndEnabled)
            {
                yield return new WaitForSecondsRealtime(Random.Range(minWait, maxWait));

                string trigger = triggers.RandomElement();
                
                _animator.SetTrigger(trigger);
            }
        }
    }
}