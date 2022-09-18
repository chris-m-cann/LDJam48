using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Util;
using Util.ObjPool;

namespace LDJam48
{
    public class SpawnOnDeath: PoolableLifecycleAwareBehaviour
    {
        [SerializeField] private List<Pair<GameObject, Vector2>> thingsToSpawn;
        [SerializeField] private bool spawnOnDisable;

        private struct Spawnable
        {
            public GameObject Prefab;
            public Vector2 Offset;
            public Action<GameObject> OnSpawn;
        }

        private List<Spawnable> _spawnables = new List<Spawnable>();

        private bool _isQuitting;
        private void OnApplicationQuit()
        {
            _isQuitting = true;
        }

        private void OnDisable()
        {
            if (spawnOnDisable)
            {
                Debug.LogError($"{gameObject.name} SpawnObjects Spawning on ondisable");
                SpawnObjects();
            }
        }

        public void Add(GameObject prefab, Vector2 offset = default, Action<GameObject> onSpawn = default)
        {
            _spawnables.Add(new Spawnable
            {
                Prefab = prefab, Offset =  offset, OnSpawn = onSpawn
            });
        }

        public void SpawnObjects()
        {
            if (_isQuitting) return;

            foreach (var thing in thingsToSpawn)
            {
                InstantiateEx.Create(thing.First, transform.position + (Vector3)thing.Second, Quaternion.identity);
            }
            
            foreach (var thing in _spawnables)
            {
                GameObject spawned = InstantiateEx.Create(thing.Prefab, transform.position + (Vector3)thing.Offset, Quaternion.identity);
                thing.OnSpawn?.Invoke(spawned);
            }

        }

        public override void OnPop()
        {
            _spawnables.Clear();
        }
    }
}