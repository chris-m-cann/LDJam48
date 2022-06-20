using UnityEngine;

namespace Util.ObjPool
{
    public static  class InstantiateEx
    {
        public static GameObject Create(GameObject prefab, Vector3 pos, Quaternion rotation)
        {
            if (ObjectPoolManager.Instance != null)
            {
                return ObjectPoolManager.Instance.Create(prefab, pos, rotation);
            }

            return GameObject.Instantiate(prefab, pos, rotation);
        }
        
        public static T Create<T>(T prefab, Vector3 pos, Quaternion rotation) where T : Component
        {
            if (ObjectPoolManager.Instance != null)
            {
                return ObjectPoolManager.Instance.Create(prefab, pos, rotation);
            }

            return GameObject.Instantiate(prefab, pos, rotation);
        }

        public static void Destroy(GameObject go)
        {
            if (go.TryGetComponent<Poolable>(out Poolable p))
            {
                p.ReturnToPool();
            }
            else
            {
                GameObject.Destroy(go);
            }
        }
    }
}