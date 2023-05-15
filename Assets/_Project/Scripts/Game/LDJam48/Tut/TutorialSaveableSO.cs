using System;
using LDJam48.Save;
using UnityEngine;

namespace LDJam48.Tut
{
    [CreateAssetMenu(menuName = "Custom/Save/Saveable/Tutorial")]
    public class TutorialSaveableSO : SaveableSOT<TutorialSaveable>
    {
        public void SetTutorialRequired(bool required)
        {
            Data.TutorialRequired = required;
        }

        public override void Reset()
        {
            base.Reset();
            Data.TutorialRequired = true;
        }
    }

    [Serializable]
    public struct TutorialSaveable : ISaveable
    {
        public bool TutorialRequired;
    }
}
