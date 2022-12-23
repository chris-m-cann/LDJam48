using System;
using System.Collections;
using LDJam48.Var.Observe;
using UnityEngine;
using UnityEngine.UI;
using Util;
using Util.Var;

namespace LDJam48
{
    public class CutoffPanel : MonoBehaviour
    {
        [SerializeField] private TweenBehaviour tween;
        [SerializeField] private Image image;
        [SerializeField] private ObservableScreenWipeVariable screenWipe;
        [SerializeField] private BoolReference wipeInOnStart ;
        [SerializeField] private BoolReference setupOnAwake ;


        private void Awake()
        {
            Debug.Log($"{name}: COP Awake");
            if (setupOnAwake.Value)
            {
                Material mat = image.material;
                mat.SetFloat("Cutoff", 0);
                image.raycastTarget = false;
                image.gameObject.SetActive(true);
            }
        }

        private void Start()
        {
            Debug.Log($"{name}: COP Start");            
            if (wipeInOnStart.Value)
            {
                WipeIn();
            }
        }

        private void OnEnable()
        {
            Debug.Log($"{name}: COP OnEnable");
            screenWipe.OnValueChanged += WipeOut;
        }

        private void OnDisable()
        {
            screenWipe.OnValueChanged -= WipeOut;
        }

        public void WipeIn()
        {
            Debug.Log($"{name}: COP WipeIn");
            if (screenWipe.Value.EffectImage == null)
            {
                Debug.LogError($"Wipe in: wipe value null");
                return;
            }
            Debug.Log($"Wiping in on start, {screenWipe.Value.EffectImage.name}, {screenWipe.Value.WipeInTime}, {screenWipe.Value.WipeOutTime}");
            
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
            
            tween.SetTween(0, inDesc);
            tween.SetTween(1, outDesc);
            
            image.raycastTarget = true;
            image.gameObject.SetActive(true);
            image.sprite = wipe.EffectImage;
        }

        private IEnumerator CoPlay(int idx)
        {
            TweenDescription t = tween.GetTween(idx);

            Debug.Log($"playing tween: {t.Duration}, {t.Start}, {t.End}: {image.sprite.name}");
            yield return StartCoroutine(tween.CoPlay(t));
            Debug.Log($"{name}: Completed tween {idx}");
            if (idx == 0)
            {
                Debug.Log($"{name}: Back from ScenePlay");
                image.raycastTarget = false;
                yield return new WaitForSeconds(.2f);
                
                image.raycastTarget = true;
                image.gameObject.SetActive(false);
            }
        }
    }
}