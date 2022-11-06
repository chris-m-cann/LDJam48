using System;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace Util.UI
{
    [RequireComponent(typeof(TMP_Text))]
    public class FpsCounter : MonoBehaviour
    {
        [SerializeField] private int averageOver;

        private TMP_Text _text;

        private float _time;
        private int _count;

        private void Awake()
        {
            _text = GetComponent<TMP_Text>();
        }

        private void Update()
        {
            _time += Time.unscaledDeltaTime;
            --_count;
            if (_count < 0)
            {
                _text.text = CalculateFps();
                _count = averageOver;
                _time = 0f;
            }
        }

        private string CalculateFps()
        {
            float fps = averageOver / _time;

            return Mathf.RoundToInt(fps).ToString();
        }
    }
}