using UnityEngine;
using UnityEngine.Events;
using Util.Var;
using Util.Var.Events;

namespace Util
{
    public class OnKeyPressEvent : MonoBehaviour
    {
        [SerializeField] private KeyCode key = KeyCode.Escape;
        [SerializeField] private UnityEvent onKeyDown;

        [SerializeField] private IntGameEventReference ier;
        [SerializeField] private FloatReference fref;


        private void Update()
        {
            if (Input.GetKeyDown(key))
            {
                onKeyDown.Invoke();
            }
        }
    }
}