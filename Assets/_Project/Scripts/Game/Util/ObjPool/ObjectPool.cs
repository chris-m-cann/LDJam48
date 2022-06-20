using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Util.ObjPool
{
    public class ObjectPool : MonoBehaviour
    {
        public GameObject prefab;

        [SerializeField] private int poolSize;
        [SerializeField] private Transform poppedParent;
        [SerializeField] private bool debugLog;
        

        private Stack<Poolable> _pool;

        private void Awake()
        {
            _pool = new Stack<Poolable>(poolSize);

            prefab.SetActive(false);
            for (int i = 0; i < poolSize; ++i)
            {
                GameObject o = Instantiate(prefab, transform);
                Poolable p = o.AddComponent<Poolable>();
                p.Init(this);

                o.name = prefab.name + " " + i + "(pooled)";

                _pool.Push(p);
            }
            prefab.SetActive(true);
        }
        
        
        public GameObject Pop(Vector3 position, Quaternion rotation)
        {
            if (_pool.Count == 0)
            {           
                Debug.Log($"Pool overflow for {prefab.name}");

                return GameObject.Instantiate(prefab, position, rotation);
            }

            var obj = _pool.Pop();

            obj.transform.SetParent(poppedParent);
            // obj.transform.position = position;
            // obj.transform.rotation = rotation;
            obj.transform.SetPositionAndRotation(position, rotation);
            
            obj.OnPop();
            obj.gameObject.SetActive(true);

            return obj.gameObject;
        }

        public void Push(Poolable obj)
        {
            if (obj.Pool != this)
            {
                GameObject.Destroy(obj.gameObject);
                return;
            }

            obj.OnPush();
            obj.gameObject.SetActive(false);
            obj.transform.SetParent(transform);

            _pool.Push(obj);
        }

    }
}