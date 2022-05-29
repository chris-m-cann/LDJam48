using System;
using UnityEngine;

namespace Util
{
    public class SingletonGameObject : MonoBehaviour
    {
        private void Awake()
        {
            GameObject go = GameObject.Find(gameObject.name);
            if (go != null && go != gameObject)
            {
                Destroy(gameObject);
            }
        }
    }
}