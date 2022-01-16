using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace LDJam48.Tut
{
    public enum TutorialId
    {
        DashLeft,
        DashRight,
        Slam,
        Pause
    }

    [Serializable]
    public struct TutorialRequest
    {
        public TutorialId Id;
        [EnumToggleButtons]
        public PlayerInputController.PlayerInputs CompletionTrigger;
        [EnumToggleButtons]
        public PlayerInputController.PlayerInputs AllowedInputsDuring;
        [EnumToggleButtons]
        public PlayerInputController.PlayerInputs AllowedInputsAfter;
        [TextArea] public string Message;
        [HideInInspector] public bool IsComplete;
    }
}