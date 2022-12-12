using System;
using System.Collections;
using LDJam48.Var.Observe;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace LDJam48
{
    public class CutoffPanel : MonoBehaviour
    {
        private static ScreenWipe? lastWipe = null;

        [SerializeField] private TweenBehaviour tween;
        [SerializeField] private Image image;
        [SerializeField] private ObservableScreenWipeVariable screenWipe;
        [SerializeField] private bool wipeInOnStart = true;

        private void Start()
        {
            if (wipeInOnStart) WipeIn();
        }

        private void OnEnable()
        {
            screenWipe.OnValueChanged += WipeOut;
        }

        private void OnDisable()
        {
            screenWipe.OnValueChanged -= WipeOut;
        }

         

        public void WipeIn()
        {
            if (screenWipe.Value.EffectImage == null) return;
            
            SetupWipe(screenWipe.Value);
            StartCoroutine(CoPlay(0));
        }

        private void WipeOut(ScreenWipe wipe)
        {
            SetupWipe(wipe);

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