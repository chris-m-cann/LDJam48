using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

namespace Util.UI
{
    [RequireComponent(typeof(TMP_Text))]
    public class TMPLinkBehaviour : MonoBehaviour, IPointerClickHandler
    {
        private TMP_Text _text;
        private Camera _camera;

        private void Awake()
        {
            _text = GetComponent<TMP_Text>();
            _camera = Camera.main;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left) return;

            Vector2 mousePos = Vector2.zero;
#if ENABLE_INPUT_SYSTEM

            if (Mouse.current != null)
            {
                mousePos = Mouse.current.position.ReadValue();
            } else if (Touch.activeTouches.Count > 0)
            {
                mousePos = Touch.activeTouches[0].screenPosition;
            }
#else
                mousePos = Input.mousePosition;
#endif
            
            int idx = TMP_TextUtilities.FindIntersectingLink(_text, mousePos, _camera);

            if (idx >= 0 && idx < _text.textInfo.linkCount)
            {
                Application.OpenURL(_text.textInfo.linkInfo[idx].GetLinkID().Trim());
            }
        }
    }
}