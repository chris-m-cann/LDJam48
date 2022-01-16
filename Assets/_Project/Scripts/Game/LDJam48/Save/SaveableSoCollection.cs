using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace LDJam48.Save
{
    [CreateAssetMenu(menuName = "Custom/Save/SaveData")]
    public class SaveableSoCollection : ScriptableObject
    {
        public SaveableSO[] Saveables;
        
        public event Action OnRequestSave;
        public event Action OnRequestLoad;
        public event Action OnLoadComplete;
        
        [HorizontalGroup("Buttons"), Button("Save")]
        public void RequestSave() => OnRequestSave?.Invoke();
        [HorizontalGroup("Buttons"), Button(Name = "Load")]
        public void RequestLoad() => OnRequestLoad?.Invoke();

        public void LoadComplete()
        {
            foreach (var saveable in Saveables)
            {
                saveable.LoadComplete();
            }
        }

        public void Enable()
        {
            foreach (var saveable in Saveables)
            {
                saveable.OnRequestSave += RequestSave;
                saveable.OnRequestLoad += RequestLoad;
            }
        }

        public void Disable()
        {
            foreach (var saveable in Saveables)
            {
                saveable.OnRequestSave -= RequestSave;
                saveable.OnRequestLoad -= RequestLoad;
            }
        }
    }
}