using System;
using System.Collections.Generic;
using UnityEngine;
using Util;

namespace LDJam48
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private Transform player;
        [SerializeField] private LevelChunk[] chunks;
        [SerializeField] private LevelChunk[] endOfLevel;
        [SerializeField] private float startHeight;
        [SerializeField] private float lookAhead;
        [SerializeField] private float levelLength;
        [SerializeField] private GameObject startObject;



        private Queue<LevelChunk> _activeChunks = new Queue<LevelChunk>();

        private float _camHeight;
        private float _furthestPoint;
        private bool _spawnedEnd;
        private bool _destroyedBegining;

        private void Awake()
        {
            _camHeight = Camera.main.orthographicSize * 2;
            _furthestPoint = startHeight;
        }

        private void Update()
        {
            DestroyOldChunks();

            CreateNewChunks();

            // removing for now as I thing we need to go endless
            // if (!_spawnedEnd && (startHeight - levelLength) > _furthestPoint)
            // {
            //     SpawnChunk(endOfLevel.RandomElement());
            //     _spawnedEnd = true;
            // }
        }

        private void CreateNewChunks()
        {
            var minY = _furthestPoint + lookAhead + _camHeight;

            if (minY > player.transform.position.y)
            {
                SpawnChunk(chunks.RandomElement());
            }
        }

        private void SpawnChunk(LevelChunk prefab)
        {
            var start = Vector2.up * _furthestPoint;
            var chunk = Instantiate(prefab, start + prefab.Offset, Quaternion.identity);
            chunk.transform.parent = transform;
            _furthestPoint -= chunk.Height;
            _activeChunks.Enqueue(chunk);
        }

        private void DestroyOldChunks()
        {
            if (!_destroyedBegining)
            {
                if (player.transform.position.y < (startHeight - _camHeight - lookAhead))
                {
                    Destroy(startObject);
                    _destroyedBegining = true;
                }
            }

            if (_activeChunks.Count == 0) return;

            var chunk = _activeChunks.Peek();
            var max = player.position.y + _camHeight;

            if (chunk.Bottom > max)
            {
                Destroy(_activeChunks.Dequeue().gameObject);
            }
        }
    }
}