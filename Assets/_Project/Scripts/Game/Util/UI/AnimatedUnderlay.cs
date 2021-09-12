using System;
using System.Numerics;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
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
        private TweenerCore<float, float, FloatOptions> _tweener2;
        private TweenerCore<Vector2, Vector2, VectorOptions> _tweener;

        private void Awake()
        {
            _text = GetComponent<TMP_Text>();
        }

        private void OnEnable()
        {
            _tweener = DOTween.To(() => GetOffset(), it => SetOffset(it), minOffset, duration).SetLoops(-1, LoopType.Yoyo).From(maxOffset)
                .SetUpdate(UpdateType.Normal, isIndependentUpdate:true);
            _tweener2 = DOTween.To(() => GetDilate(), it => SetDilate(it), minDilate, duration).SetLoops(-1, LoopType.Yoyo).From(maxDilate)
                .SetUpdate(UpdateType.Normal, isIndependentUpdate:true);

            _tweener.Play();
            _tweener2.Play();
        }

        private void OnDisable()
        {
            _tweener.Kill();
            _tweener2.Kill();
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