using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Util
{
    [Serializable, DrawWithUnity]
    public class OneOf
    {
        [SerializeField] protected byte Delimeter;
    }
}