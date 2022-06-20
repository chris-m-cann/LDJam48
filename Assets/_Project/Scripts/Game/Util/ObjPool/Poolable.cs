using System;
using System.Collections.Generic;
using UnityEngine;

namespace Util.ObjPool
{
    public class Poolable : MonoBehaviour
    {
        public ObjectPool Pool;

        private PoolableLifecycleAware[] _poolAwareComponents;

        private PoolableLifecycleAware[] _PoolAwareComponents
        {
            get
            {
                if (_poolAwareComponents == null)
                {
                    _poolAwareComponents = GetComponentsInChildren<PoolableLifecycleAware>();
                }

                return _poolAwareComponents;
            }
        }
        

        public void Init(ObjectPool pool)
        {
            Pool = pool;
        }

        public void ReturnToPool()
        {
            Pool.Push(this);
        }
        
        public void OnPush()
        {
            foreach (var component in _PoolAwareComponents)
            {
                component.OnPush();
            }
        }

        public void OnPop()
        {
            foreach (var component in _PoolAwareComponents)
            {
                component.OnPop();
            }
        }
    }
}