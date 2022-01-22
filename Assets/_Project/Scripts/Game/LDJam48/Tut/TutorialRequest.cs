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
        // perhaps a more extensible way to do this would be to have an id that does a lookup based on enabled
        // control scheme but as we only have 2 that might be overkill
        [TextArea] public string TouchMessage;
        [TextArea] public string KeyboardMessage;
        [HideInInspector] public bool IsComplete;
    }
}