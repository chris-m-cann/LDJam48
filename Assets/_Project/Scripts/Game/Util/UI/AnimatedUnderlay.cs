using System;
using System.Numerics;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

namespace Util.UI
{
    [RequireComponent(typeof(TMP_Text))]
    public class AnimatedUnderlay : MonoBehaviour
    {
        [SerializeField] private float duration = 1f;
        [SerializeField] private Vector2 minOffset = new Vector2(.5f, -.5f);
        [SerializeField] private Vector2 maxOffset = new Vector2(1f, -1f);
        [SerializeField] private float minDilate;
        [SerializeField] private float maxDilate = 1f;
        

        private TMP_Text _text;

        private void Awake()
        {
            _text = GetComponent<TMP_Text>();
        }

        private void OnEnable()
        {
            var tweener = DOTween.To(() => GetOffset(), it => SetOffset(it), minOffset, duration).SetLoops(-1, LoopType.Yoyo);
            tweener.startValue = maxOffset;
            var tweener2 = DOTween.To(() => GetDilate(), it => SetDilate(it), minDilate, duration).SetLoops(-1, LoopType.Yoyo);
            tweener2.startValue = maxDilate;

            tweener.Play();
            tweener2.Play();
        }

        private float GetDilate()
        {
            return _text.fontMaterial.GetFloat("_UnderlayDilate");
        }

        private void SetDilate(float next)
        {
            _text.fontMaterial.SetFloat("_UnderlayDilate", next);
        }

        private Vector2 GetOffset()
        {
            var x = _text.fontMaterial.GetFloat("_UnderlayOffsetX");
            var y = _text.fontMaterial.GetFloat("_UnderlayOffsetY");

            return new Vector2(x, y);
        }

        private void SetOffset(Vector2 next)
        {
            _text.fontMaterial.SetFloat("_UnderlayOffsetX", next.x);
            _text.fontMaterial.SetFloat("_UnderlayOffsetY", next.y);
        }
    }
}