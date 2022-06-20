using System;
using UnityEngine;
using Util.ObjPool;

namespace Util
{
    public class DestroyBehaviour : MonoBehaviour
    {
        public void DestroyGameObject(GameObject go)
        {
            InstantiateEx.Destroy(go);
        }

        public void DestroyGameObject() => DestroyGameObject(gameObject);
    }
}