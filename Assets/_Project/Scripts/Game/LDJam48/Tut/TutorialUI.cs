using System;
using LDJam48.Var.Events;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Util;
using Util.Var;

namespace LDJam48.Tut
{
    public class TutorialUI : MonoBehaviour
    {
        [Header("Indicators")]
        [Tooltip("Indicator indices:\n0: pause\n1: dash left\n2: dash right\n3: slam")]
        [SerializeField] private Pair<Image, Animator>[] mobile;
        [Tooltip("Indicator indices:\n0: pause\n1: dash left\n2: dash right\n3: slam")]
        [SerializeField] private Pair<Image, Animator>[] keyboard;
        
        [Space(height:20)]
        [SerializeField] private TMP_Text message;
        [SerializeField] private GameObject ui;
        [SerializeField] private TutorialRequestEventReference tutorialChannel;
        [SerializeField] private BoolReference isMobileControls;

        [SerializeField] private UnityEvent onTutorialOpen;
        [SerializeField] private UnityEvent onTutorialClose;

        private void EnableIndicator(Pair<Image, Animator> indicator)
        {
                indicator.First.color = Color.white;
                indicator.Second.enabled = true;
        }
        
        private void DisableIndicator(Pair<Image, Animator> indicator)
        {
            indicator.First.color = Color.clear;
            indicator.Second.enabled = false;
        }

        private void DisplayText(TutorialRequest request)
        {
            if (isMobileControls.Value)
            {
                message.text = request.TouchMessage;
            }
            else
            {
                message.text = request.KeyboardMessage;    
            }
            
        }

        private void OnEnable()
        {
            ui.SetActive(false);
            tutorialChannel.OnEventTriggered += OnTutorialRequest;
        }

        private void OnDisable()
        {
            tutorialChannel.OnEventTriggered -= OnTutorialRequest;
        }

        private void OnTutorialRequest(TutorialRequest tutorialRequest)
        {
            if (tutorialRequest.IsComplete)
            {
                DisableTutorial(tutorialRequest);
            }
            else
            {
                EnableTutorial(tutorialRequest);
            }
        }

        private void EnableTutorial(TutorialRequest tutorialRequest)
        {
            // change text
            DisplayText(tutorialRequest);
            // enable ui
            ui.SetActive(true);
            // enable indicator
            var indicators = isMobileControls.Value ? mobile : keyboard;
            
            switch (tutorialRequest.Id)
            {
                case TutorialId.Pause:
                    EnableIndicator(indicators[0]);
                    break;
                case TutorialId.DashLeft:
                    EnableIndicator(indicators[1]);
                    break;
                case TutorialId.DashRight:
                    EnableIndicator(indicators[2]);
                    break;
                case TutorialId.Slam:
                    EnableIndicator(indicators[3]);
                    break;
                default:
                    break;
            }
            
            onTutorialOpen?.Invoke();
        }

        private void DisableTutorial(TutorialRequest tutorialRequest)
        {
            ui.SetActive(false);
            var indicators = isMobileControls.Value ? mobile : keyboard;
            foreach (var i in indicators)
            {
                DisableIndicator(i);
            }
            
            onTutorialClose?.Invoke();
        }
    }
}