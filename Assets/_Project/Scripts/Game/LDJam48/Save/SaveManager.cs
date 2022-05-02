using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Util;

namespace LDJam48.Save
{
    public class SaveManager : DontDestroyOnLoad
    {
        [SerializeField] private SaveableSoCollection saveData;
        [SerializeField] private SaveStorageSo storage;
        [SerializeField] private bool loadOnStart = true;

        private void OnEnable()
        {
            saveData.OnRequestSave += Save;
            saveData.OnRequestLoad += Load;
            saveData.Enable();
        }

        private void OnDisable()
        {
            saveData.OnRequestSave -= Save;
            saveData.OnRequestLoad -= Load;
            saveData.Disable();
        }

        private void Start()
        {
            // is in start as certain things (namely AuioMixer.SetFloat) wont work until start
            if (loadOnStart)
            {
                Load();
            }
        }

        [HorizontalGroup("Buttons"), Button("Save")]
        public void Save()
        {
            storage.Save(saveData);
        }

        [HorizontalGroup("Buttons"), Button("Load")]
        public void Load()
        {
            bool complete = storage.Load(saveData);

            if (!complete)
            {
                Debug.LogError($"Saving {saveData.name} failed");
                return;
            }
            
            saveData.LoadComplete();
        }
    }
}