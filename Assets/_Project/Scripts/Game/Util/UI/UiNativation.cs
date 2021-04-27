using UnityEngine;
using UnityEngine.EventSystems;

namespace Util.UI
{
    public class UiNativation : MonoBehaviour
    {
        [SerializeField] private EventSystem eventSystem;
        [SerializeField] private GameObject firstSelected;
        [SerializeField] private string[] axes;


        private void Update()
        {
            if (eventSystem.currentSelectedGameObject != null) return;

            foreach (var axis in axes)
            {
                if (Input.GetAxis(axis) != 0)
                {
                    eventSystem.SetSelectedGameObject(firstSelected);

                    break;
                }
            }
        }

        public void PrintMessage(string message) => Debug.Log(message);
    }
}
