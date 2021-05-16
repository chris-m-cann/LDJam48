using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Util;
using Util.Var.Events;

namespace LDJam48
{
    public class SwipeController : MonoBehaviour
    {
        [SerializeField] private float minSwipeDistance;
        [SerializeField] private float maxTapTime;
        [SerializeField] private float swipeIdleTime;
        [SerializeField] private float deadZone;
        [SerializeField] private Vector2GameEvent onTap;
        [SerializeField] private Vector2GameEvent onSwipe;


        [SerializeField] private SpriteRenderer indicator;
        [SerializeField] private bool drawIndicator;

        [SerializeField] private InputActionReference actionReference;




        private Vector2 _downPos;
        private Vector2 _currMousePos;
        private Vector2 _lastMousePos;
        private Vector2 _deltaMousePos;
        private float _downTime;
        private bool _isDown;

        private Camera _camera;
        private float _swipeCooldown;
        private bool _canSwipe;
        private bool _canTap;

        private bool GetMouseDown() => Mouse.current.leftButton.wasPressedThisFrame;
        private bool GetMouseUp() => Mouse.current.leftButton.wasReleasedThisFrame;

        private Vector2 GetMousePosition()
        {
            return Mouse.current.position.ReadValue();
        }
        private void Start()
        {
            _camera = Camera.main;


        }

        private void OnGUI()
        {
            GUI.Label(new Rect(0, 0, 30, 20), $"{_deltaMousePos.magnitude}");
        }

        private void OnValidate()
        {
            indicator?.gameObject?.SetActive(drawIndicator);
        }

        private void Update()
        {
            try
            {
                _currMousePos = GetMousePosition();
                UpdateDeltaMousePos();
                UpdateIndicator();

                if (GetMouseDown() && !_isDown)
                {
                    _isDown = true;
                    _downPos = _currMousePos;
                    _downTime = Time.time;
                    _canSwipe = true;
                    _canTap = true;
                }

                var hasSwiped = false;
                if (_isDown)
                {
                    hasSwiped = DetectSwipe();
                }

                if (_canTap && hasSwiped)
                {
                    _canTap = false;
                }


                if (GetMouseUp())
                {
                    _isDown = false;

                    DetectTap();
                }
            }
            finally
            {
                _lastMousePos = _currMousePos;
            }
        }

        private void UpdateDeltaMousePos()
        {
            _deltaMousePos = (Vector2) _currMousePos - _lastMousePos;

            _deltaMousePos.x /= Screen.width;
            _deltaMousePos.y /= Screen.height;
        }



        private void UpdateIndicator()
        {
            if (indicator && drawIndicator)
            {
                var pos = _camera.ScreenToWorldPoint(_currMousePos);
                pos.z = 0;
                indicator.transform.position = pos;
            }
        }

        private void DetectTap()
        {
            var tapTime = Time.time - _downTime;
            var delta = (Vector2) _currMousePos - _downPos;
            delta.x /= Screen.width;
            delta.y /= Screen.height;

            if (_canTap && tapTime < maxTapTime && delta.magnitude < deadZone)
            {
                OnTap();
            }
        }

        private void OnTap()
        {
            // done a tap!
            onTap?.Raise(_downPos);

            // debug drawing
            var current = indicator.color.a;
            var colour = indicator.color;
            colour.a = 1;
            indicator.color = colour;

            this.ExecuteAfter(.5f, () =>
            {
                var colour = indicator.color;
                colour.a = current;
                indicator.color = colour;
            });
        }

        private bool DetectSwipe()
        {
            // idle cooldown handling
            if (_deltaMousePos.magnitude > deadZone)
            {
                _swipeCooldown = Time.time;
            }
            else
            {
                // if i havnt moved by the deadzone amount consider
                // this my new start pos for subsequent swipes
                _downPos = _currMousePos;
            }


            _canSwipe = _canSwipe || Time.time - _swipeCooldown > swipeIdleTime;

            if (!_canSwipe) return false;

            var delta = ((Vector2) _currMousePos - _downPos);
            delta.x /= Screen.width;
            delta.y /= Screen.height;

            if (delta.magnitude > minSwipeDistance)
            {
                OnSwipe();
                return true;
            }

            return false;
        }


        private void OnSwipe()
        {
            Debug.Log("Done a Swipe!");
            _canSwipe = false;
            _swipeCooldown = Time.time;

            // a swipe happened!
            onSwipe?.Raise((Vector2) _currMousePos - _downPos);


            // draw debug
            var start = _camera.ScreenToWorldPoint(_downPos);
            start.z = 0;

            var end = _camera.ScreenToWorldPoint(_currMousePos);
            end.z = 0;
            Debug.DrawLine(start, end, Color.red, .6f);
        }
    }

}