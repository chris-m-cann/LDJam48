using UnityEngine;
using UnityEngine.Events;

namespace LDJam48
{
    public class SideMarker : MonoBehaviour
    {
        [SerializeField] private UnityEvent onPassed;
        
        public void OnPassedMarker()
        {
            onPassed?.Invoke();
        }
    }
}