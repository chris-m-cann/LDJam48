using UnityEngine;
using UnityEngine.Events;
using Util.Var.Observe;

namespace LDJam48
{
    public class OnPauseResume : MonoBehaviour
    {
        [SerializeField] private ObservableStringVariable activeActionMap;
        [SerializeField] private string pauseActionMap = "Menu";
        [SerializeField] private UnityEvent onPause;
        [SerializeField] private UnityEvent onResume;


        private string _prevMap;


        public void OnPause()
        {
            _prevMap = activeActionMap.Value;
            activeActionMap.Value = pauseActionMap;
            onPause?.Invoke();
        }

        public void OnResume()
        {
            activeActionMap.Value = _prevMap;
            _prevMap = pauseActionMap;
            onResume?.Invoke();
        }

    }
}