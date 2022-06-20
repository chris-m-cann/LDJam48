using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

namespace Util.ObjPool
{
    public class ObjectPoolManager : MonoBehaviour
    {
        public static ObjectPoolManager Instance;


        private Dictionary<GameObject, ObjectPool> _pools = new Dictionary<GameObject, ObjectPool>();
        private void Awake()
        {
            if (Instance == null) {
                Instance = this;
                
                Init();
            } else {
                Destroy (gameObject);
            }
        }
        
        private void Init()
        {
            var pools = FindObjectsOfType<ObjectPool>();
            foreach (var pool in pools)
            {
                _pools[pool.prefab] = pool;
            }
        }

        public GameObject Create(GameObject prefab, Vector3 pos, Quaternion rotation)
        {
            if (_pools.ContainsKey(prefab))
            {
                return _pools[prefab].Pop(pos, rotation);
            }
            
            Debug.Log($"Couldnt find pool for {prefab.name}");

            return Instantiate(prefab, pos, rotation);
        }
        
        public T Create<T>(T prefab, Vector3 pos, Quaternion rotation) where T : Component
        {
            if (_pools.ContainsKey(prefab.gameObject))
            {
                return _pools[prefab.gameObject].Pop(pos, rotation).GetComponent<T>();
            }
            
            Debug.Log($"Couldnt find pool for {prefab.gameObject.name}");

            return Instantiate(prefab, pos, rotation);
        }
    }
    
}