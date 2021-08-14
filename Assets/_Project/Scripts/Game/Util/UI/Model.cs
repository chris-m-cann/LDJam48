using System;
using System.Reflection;
using UnityEngine;

namespace Util.UI
{
    public class Model : ScriptableObject
    {
        [NonSerialized] public Lazy<FieldInfo[]> Fields;

        public Model()
        {
            Fields = new Lazy<FieldInfo[]>(GetFields);
        }

        private FieldInfo[] GetFields()
        {
            return ModelBinding.GetBindableFields(GetType());
        }
    }
}