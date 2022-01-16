using System;
using LDJam48.Var.Events;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace LDJam48.Tut
{
    public class TutorialUI : MonoBehaviour
    {

        [SerializeField] private Pair<Image, Animator>[] indicators;
        [SerializeField] private TMP_Text message;
        [SerializeField] private GameObject ui;
        [SerializeField] private TutorialRequestEventReference tutorialChannel;
        

        private void EnableIndicator(int idx)
        {
            indicators[idx].First.color = Color.white;
            indicators[idx].Second.enabled = true;
        }

        private void DisableIndicator(int idx)
        {
            indicators[idx].First.color = Color.clear;
            indicators[idx].Second.enabled = false;
        }

        private void DisplayText(string msg)
        {
            message.text = msg;
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
            DisplayText(tutorialRequest.Message);
            // enable ui
            ui.SetActive(true);
            // enable indicator
            switch (tutorialRequest.Id)
            {
                case TutorialId.Pause:
                    EnableIndicator(0);
                    break;
                case TutorialId.DashLeft:
                    EnableIndicator(1);
                    break;
                case TutorialId.DashRight:
                    EnableIndicator(2);
                    break;
                case TutorialId.Slam:
                    EnableIndicator(3);
                    break;
                default:
                    break;
            }
        }

        private void DisableTutorial(TutorialRequest tutorialRequest)
        {
            ui.SetActive(false);
            for (int i = 0; i < indicators.Length; i++)
            {
                DisableIndicator(i);
            }
        }
    }
}