using LDJam48.PlayerState;
using UnityEngine;
using UnityEngine.Events;

namespace LDJam48
{
    public class Bounceable : MonoBehaviour
    {
        [SerializeField] private UnityEvent onBounce;

        private PlayerStateMachine _psm;

        private void Awake()
        {
            _psm = GetComponent<PlayerStateMachine>();
        }

        public void Bounce()
        {
            onBounce?.Invoke();
            if (_psm != null)
            {
                _psm.Bounce();
            }
        }
    }
}