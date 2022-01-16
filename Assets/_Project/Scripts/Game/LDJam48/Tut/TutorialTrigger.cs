using System;
using LDJam48.LevelGen;
using LDJam48.Var.Events;
using UnityEngine;

namespace LDJam48.Tut
{
    public class TutorialTrigger : MonoBehaviour
    {
        [SerializeField] private TutorialRequest tutorial;
        [SerializeField] private TutorialRequestEventReference tutorialRequestChannel;

        private TimeEx _pauser;
        private PlayerInputController _playerInput;

        private bool _tutorialActive = false;
        private int _timeStopId;
        private void Awake()
        {
            _pauser = FindObjectOfType<TimeEx>();
            _playerInput = FindObjectOfType<PlayerInputController>();
        }

        public void OnTutorialTrigger()
        {
            _timeStopId = _pauser.PushTimeScale(0f);
            _playerInput.AllowedInputs = tutorial.AllowedInputsDuring;

            tutorial.IsComplete = false;
            tutorialRequestChannel?.Raise(tutorial);
            _tutorialActive = true;
        }

        public void OnTutorialComplete()
        {
            if (!_tutorialActive) return;
            _playerInput.AllowedInputs = tutorial.AllowedInputsAfter;
            _pauser.PopTimeScale(_timeStopId);
            
            tutorial.IsComplete = true;
            tutorialRequestChannel?.Raise(tutorial);

            _tutorialActive = false;
        }
    }
}