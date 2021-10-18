using UnityEngine;

namespace LDJam48
{
    [RequireComponent(typeof(Animator))]
    public class SetAnimatorParameter : MonoBehaviour
    {
        [SerializeField] private string parameterName;
        [SerializeField] private int randomIntMin;
        [SerializeField] private int randomIntMax;
        
        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public void SetRandomIntParameter()
        {
            _animator.SetInteger(parameterName, Random.Range(randomIntMin, randomIntMax + 1));
        }
        
    }
}