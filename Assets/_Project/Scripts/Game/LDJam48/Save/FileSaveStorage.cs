using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using Sirenix.Serialization;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LDJam48.Save
{
    [CreateAssetMenu(menuName = "Custom/Save/Storage/File")]
    public class FileSaveStorage : SaveStorageSo
    {
        [SerializeField] private string filename;

        public override bool Save(SaveableSoCollection saveables)
        {
            try
            {
                Dictionary<string, ISaveable>
                    dictionary = new Dictionary<string, ISaveable>(saveables.Saveables.Length);

                foreach (var saveable in saveables.Saveables)
                {
                    dictionary.Add(saveable.Key, saveable.GetSaveable());
                }
                
                byte[] bytes = SerializationUtility.SerializeValue(dictionary, DataFormat.JSON);
                string json = Encoding.UTF8.GetString(bytes);
                var path = GetPath();

                using FileStream file = File.Create(path);
                file.Write(bytes, 0, bytes.Length);

                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"error saving {saveables.name}: " + e);
                return false;
            }
        }

        public override bool Load(SaveableSoCollection saveables)
        {
            try
            {
                byte[] bytes = File.ReadAllBytes(GetPath());
                string json = Encoding.UTF8.GetString(bytes);
                Dictionary<string, ISaveable> dictionary = SerializationUtility.DeserializeValue<Dictionary<string, ISaveable>>(bytes, DataFormat.JSON);

                foreach (var saveable in saveables.Saveables)
                {
                    if (!dictionary.ContainsKey(saveable.Key)) continue;
                    
                    saveable.SetSaveable(dictionary[saveable.Key]);
                }
                
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"error loading {saveables.name}: " + e);
                return false;
            }
        }
        private string GetPath()
        {
            string path = Application.persistentDataPath + "/" + filename + ".json";
            return path;
        }
        
    }
}