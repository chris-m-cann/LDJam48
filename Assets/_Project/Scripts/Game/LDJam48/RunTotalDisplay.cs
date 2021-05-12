using System;
using System.Collections;
using DG.Tweening;
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
        [SerializeField] private float depthDrainTime = 1f;
        [SerializeField] private float gemsDrainTime = 1f;



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


            var sequence = DOTween.Sequence();
            sequence.SetUpdate(UpdateType.Normal, true);
            sequence.Append(
                DOTween.To(() => _total, it =>
                {
                    _total = it;
                    SetTotalText();
                }, depth.Value, depthDrainTime).From(0));

            sequence.AppendInterval(betweenDelay);
                sequence.Append(
                DOTween.To(()=>_total, it =>
                {
                    _total = it;
                    SetTotalText();
                }, depth.Value + totalGems.Value, gemsDrainTime).From(depth.Value)
            );

            sequence.Play();
        }

        private void SetTotalText()
        {
            foreach (var text in _texts)
            {
                text.text = _total.ToString();
            }
        }
    }
}