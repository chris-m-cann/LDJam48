using System;
using UnityEngine;

namespace Util
{
    public class DestroyBehaviour : MonoBehaviour
    {
        public void DestroyGameObject(GameObject go)
        {
            Destroy(go);
        }

        public void DestroyGameObject() => DestroyGameObject(gameObject);
    }
}