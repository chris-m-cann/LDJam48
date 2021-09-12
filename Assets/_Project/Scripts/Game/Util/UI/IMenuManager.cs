using System;
using UnityEngine;

namespace Util.UI
{
    public interface IMenuManager
    {
        void OpenMenu();
        void OpenMenuTo(int menuIdx);
        void ToggleMenu();
        void SwitchMenu(SwapMenuAction action);

        void PopBackstack();
        void CloseMenu();
    }

    [Serializable]
    public struct SwapMenuAction
    {
        // menu action transitions from
        [HideInInspector] public int SourceMenu;
        
        [Tooltip("Index of the target destination menu")]
        public int Destination;
        [Tooltip("transition on this menu when it is exited due to this action")]
        public int OnExitTransition;
        [Tooltip("transition on this menu when it is exited due to this action")]
        public int OnPopBackToTransition;
        
        
        [Tooltip("transition on the destination menu when you move it it due to this action")]
        public int OnEnterTransition;
        [Tooltip("transition on the destination menu when pop is called from it")]
        public int OnPopFromTransition;
    }
}