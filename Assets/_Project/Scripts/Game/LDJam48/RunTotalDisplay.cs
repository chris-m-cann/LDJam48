using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using Util.Var;
using Util.Var.Observe;

namespace LDJam48
{
    public class RunTotalDisplay : MonoBehaviour
    {
        [SerializeField] private ObservableIntVariable depth;
        [SerializeField] private ObservableIntVariable totalGems;
        [SerializeField] private float initialDelay = 1f;
        [SerializeField] private float betweenDelay = .5f;
        [SerializeField] private float depthDrainRate = 50f;
        [SerializeField] private float gemsDrainRate = 80f;


        private TMP_Text[] _texts;
        private int _total;

        private void Awake()
        {
            _texts = GetComponentsInChildren<TMP_Text>();
        }

        private void OnEnable()
        {
            StartCoroutine(CoDrainScore());
        }

        private IEnumerator CoDrainScore()
        {
            _total = 0;

            SetTotalText();

            yield return new WaitForSecondsRealtime(initialDelay);

            yield return StartCoroutine(CoDrainDepth());
            yield return new WaitForSecondsRealtime(betweenDelay);
            yield return StartCoroutine(CoDrainGems());
        }

        private void SetTotalText()
        {
            foreach (var text in _texts)
            {
                text.text = _total.ToString();
            }
        }

        private IEnumerator CoDrainDepth()
        {
            while (depth.Value > _total)
            {
                yield return new WaitForSecondsRealtime(1 / depthDrainRate);
                _total += 1;
                SetTotalText();
            }
        }

        private IEnumerator CoDrainGems()
        {
            var finalTotal = _total + totalGems.Value;
            while (finalTotal > _total)
            {
                yield return new WaitForSecondsRealtime(1 / gemsDrainRate);
                _total += 1;
                SetTotalText();
            }
        }
    }
}