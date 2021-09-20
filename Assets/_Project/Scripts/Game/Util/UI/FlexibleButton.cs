using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Util.UI
{
    public class FlexibleButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private SpriteState states;
        [Header("State Events")]
        [SerializeField] private ButtonState up = new ButtonState(ButtonStateType.Up);
        [SerializeField] private ButtonState down = new ButtonState(ButtonStateType.Down);
        [SerializeField] private ButtonState highlighted = new ButtonState(ButtonStateType.Highlighted);
        [SerializeField] private ButtonState disabled = new ButtonState(ButtonStateType.Disabled);

        [SerializeField] private UnityEvent onClick;
        
        

        private Image _image;
        private Sprite _upSprite;
        private TMP_Text _text;
        private ButtonState _current;
        private ButtonState _prev;
        private bool _interactable = true;
        private float _exitTime;
        private float _exitWindow= .1f;

        private void Awake()
        {
            _image = GetComponentInChildren<Image>();
            _upSprite = _image.sprite;
            _text = GetComponentInChildren<TMP_Text>();
            _current = up;
        }

        private void Start()
        {
            up.OnStateEnter.AddListener(e=> _image.sprite = _upSprite);
            up.Tranisitions.Add(ButtonEvent.PointerEnter, highlighted);
            up.Tranisitions.Add(ButtonEvent.PointerDown, down);
            up.Tranisitions.Add(ButtonEvent.Disable, disabled);

            down.OnStateEnter.AddListener(e => _image.sprite = states.pressedSprite);
            down.Tranisitions.Add(ButtonEvent.PointerUp, up);
            down.Tranisitions.Add(ButtonEvent.PointerExit, up);
            down.Tranisitions.Add(ButtonEvent.Disable, disabled);
            
            highlighted.OnStateEnter.AddListener(e => _image.sprite = states.highlightedSprite);
            highlighted.Tranisitions.Add(ButtonEvent.PointerExit, up);
            highlighted.Tranisitions.Add(ButtonEvent.PointerDown, down);
            highlighted.Tranisitions.Add(ButtonEvent.Disable, disabled);
            
            disabled.OnStateEnter.AddListener(e => _image.sprite = states.disabledSprite);
            disabled.Tranisitions.Add(ButtonEvent.Enable, up);
        }

        private void OnEnable()
        {
            _current.OnStateEnter?.Invoke(ButtonEvent.ComponentEnabled);
        }

        
        private void OnDisable()
        {
            _current.OnStateEnter?.Invoke(ButtonEvent.ComponentDisabled);
        }


        private void OnEvent(ButtonEvent e)
        {
            if (_current.Tranisitions.ContainsKey(e))
            {
                
                _prev = _current;
                _current = _current.Tranisitions[e];
                
                _prev.OnStateExit?.Invoke(e);
                _current.OnStateEnter?.Invoke(e);
            }
        }


        public void OnPointerDown(PointerEventData eventData)
        {
            OnEvent(ButtonEvent.PointerDown);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            var wasDown = _current.Type == ButtonStateType.Down;
            var exitWasWithinWindow = Mathf.Abs(Time.unscaledTime - _exitTime) < _exitWindow;
            if ((wasDown ||exitWasWithinWindow))
            {
                onClick?.Invoke();
            }
            
            
            OnEvent(ButtonEvent.PointerUp);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            OnEvent(ButtonEvent.PointerEnter);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _exitTime = Time.unscaledTime;
            OnEvent(ButtonEvent.PointerExit);
        }

        public void SetInteractable(bool isInteractable)
        {
            if (isInteractable == _interactable) return;

            var @event = _interactable ? ButtonEvent.Disable : ButtonEvent.Enable;
            _interactable = isInteractable;
            OnEvent(@event);
        }

        [ContextMenu("SetInteractable(true)")]
        public void InteractableOn() => SetInteractable(true);
        
        
        [ContextMenu("SetInteractable(false)")]
        public void InteractableOff() => SetInteractable(false);
        
        public enum ButtonStateType
        {
            Up, Down, Highlighted, Disabled
        }

        public enum ButtonEvent
        {
            Enable, Disable, PointerEnter, PointerExit, PointerDown, PointerUp, ComponentEnabled, ComponentDisabled
        }
        
        [Serializable]
        public class ButtonState
        {
            public UnityEvent<ButtonEvent> OnStateEnter;
            public UnityEvent<ButtonEvent> OnStateExit;
            
            [HideInInspector]
            public ButtonStateType Type;


            [HideInInspector] public Dictionary<ButtonEvent, ButtonState> Tranisitions;

            public ButtonState(ButtonStateType type, Dictionary<ButtonEvent, ButtonState> transitions)
            {
                Type = type;
                Tranisitions = transitions;

                OnStateEnter = new UnityEvent<ButtonEvent>();
                OnStateExit = new UnityEvent<ButtonEvent>();
            }
            
            public ButtonState(ButtonStateType type) : this(type, new Dictionary<ButtonEvent, ButtonState>())
            {
            }
        }
    }


}