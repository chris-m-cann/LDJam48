using System;
using UnityEngine;

namespace LDJam48.Save
{
    [CreateAssetMenu(menuName = "Custom/Save/Saveable/Test")]
    public class TestSaveable : SaveableSOT<TestSaveStruct>
    { }

    [Serializable]
    public struct TestSaveStruct : ISaveable
    {
        public int i;
        public float f;
        public string str;
    }
}