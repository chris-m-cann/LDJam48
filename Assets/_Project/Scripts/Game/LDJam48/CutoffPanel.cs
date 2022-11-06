using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace LDJam48
{
    public class CutoffPanel : MonoBehaviour
    {
        private static ScreenWipe? lastWipe = null;
        [Serializable]
        public struct ScreenWipe
        {
            public Sprite EffectImage;
            public float WipeInTime;
            public float WipeOutTime;
        }

        [SerializeField] private TweenBehaviour tween;
        [SerializeField] private Image image;
        [SerializeField] private ScreenWipe[] screenWipes;
        [SerializeField] private bool wipeInOnStart = true;

        private void Start()
        {
            if (wipeInOnStart) WipeIn();
        }

        public void WipeIn()
        {            
            if (lastWipe.HasValue)
            {
                SetupWipe(lastWipe.Value);
            }
            else
            {
                SetupWipe(screenWipes.RandomElement());
            }

            StartCoroutine(CoPlay(0));
        }

        public void WipeOut()
        {
            SetupWipe(screenWipes.RandomElement());

            tween.Play(1);
        }

        private void SetupWipe(ScreenWipe wipe)
        {
            
            TweenDescription inDesc = tween.GetTween(0);
            TweenDescription outDesc = tween.GetTween(1);


            inDesc.Duration = wipe.WipeInTime;
            outDesc.Duration = wipe.WipeOutTime;
            
            image.gameObject.SetActive(true);
            image.sprite = wipe.EffectImage;
            lastWipe = wipe;
        }

        private IEnumerator CoPlay(int idx)
        {
            TweenDescription t = tween.GetTween(idx);

            yield return StartCoroutine(tween.CoPlay(t));
            yield return new WaitForSeconds(.5f);
            image.gameObject.SetActive(false);
        }
    }
}