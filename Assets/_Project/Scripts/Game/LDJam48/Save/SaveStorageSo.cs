using UnityEngine;

namespace LDJam48.Save
{
    public abstract class SaveStorageSo : ScriptableObject
    {
        public abstract bool Save(SaveableSoCollection saveables);
        public abstract bool Load(SaveableSoCollection saveables);
    }
}