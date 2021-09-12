using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Util.UI
{
    [RequireComponent(typeof(RectTransform))]
    public class UiWobble : MonoBehaviour
    {
        [SerializeField] private float amplitude;
        [SerializeField] private float frequency;
        [SerializeField] private float speed = 1;
        
        private RectTransform _rect;
        private Vector2 _initial;
        private void Awake()
        {
            _rect = GetComponent<RectTransform>();
        }

        private void OnEnable()
        {
            _initial = _rect.anchoredPosition;
            StartCoroutine(CoWobble());
        }

        private void OnDisable()
        {
            StopAllCoroutines();
            _rect.anchoredPosition = _initial;
        }

        private IEnumerator CoWobble()
        {
            
            var next = Time.unscaledTime + (1 / frequency);
            Vector2 destination = _rect.anchoredPosition;
            Vector2 vel = Vector2.zero;
            while (isActiveAndEnabled)
            {
                if (Time.unscaledTime > next)
                {
                    next = Time.unscaledTime + (1 / frequency);

                    // todo(chris) move to perlin noise?
                    destination = _initial + Random.insideUnitCircle * amplitude;
                }

                _rect.anchoredPosition = Vector2.SmoothDamp(_rect.anchoredPosition, destination, ref vel, speed, float.MaxValue, Time.unscaledDeltaTime);
                yield return null;
            }
        }
    }
}