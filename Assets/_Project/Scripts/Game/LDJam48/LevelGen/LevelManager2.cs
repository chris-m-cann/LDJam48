using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using Util.Var;

namespace LDJam48.LevelGen
{
    [RequireComponent(typeof(LevelAssembler))]
    public class LevelManager2 : MonoBehaviour
    {
        [SerializeField] private float activeWindowTop;
        [SerializeField] private float activeWindowBottom;
        [SerializeField] private Transform target;
        [SerializeField] private LevelChunk startChunk;
        [SerializeField] private ChunkGenerator chunkGenerator;


        private float _topChunkEnd;
        private float _nextChunkStart;


        private Queue<LevelChunk> _activeChunks = new Queue<LevelChunk>();


        private GenerationData _generationData = new GenerationData();


        private void Start()
        {
            _activeChunks.Enqueue(startChunk);

            _nextChunkStart = startChunk.Bottom;
            _topChunkEnd = _nextChunkStart;
            chunkGenerator.Init(_generationData);
        }

        private void OnDrawGizmosSelected()
        {
            if (target == null) return;

            Gizmos.color = Color.blue;

            var targetPos = target.position;
            var top = targetPos.y + activeWindowTop;
            var bottom = targetPos.y - activeWindowBottom;
            var height = activeWindowTop + activeWindowBottom;
            var center = bottom + height / 2f;



            Gizmos.DrawWireCube(
                center:new Vector3(targetPos.x, center, targetPos.z),
                size:new Vector3(10, height, 1)
            );


            Gizmos.color = Color.red;
            Gizmos.DrawSphere(
                center:new Vector3(targetPos.x, _topChunkEnd, targetPos.z), .3f);


            Gizmos.color = Color.green;
            Gizmos.DrawSphere(
                center:new Vector3(targetPos.x, _nextChunkStart, targetPos.z), .3f);
        }

        private void Update()
        {
            if (target == null) return;

            var targetY = target.position.y;

            var top = targetY + activeWindowTop;
            var bottom = targetY - activeWindowBottom;

            if (_activeChunks.Count > 0 && top < _topChunkEnd)
            {
                DestroyChunk(_activeChunks.Dequeue());
            }

            if (_nextChunkStart > bottom)
            {
                var prefab = chunkGenerator.GenerateNext(_generationData);
                var instance = BuildChunk(prefab);
                AppendChunk(instance);
            }
        }

        private void AppendChunk(LevelChunk chunk)
        {
            var t = chunk.transform;
            var p = t.position;
            p.y = _nextChunkStart;
            t.position = p;

            _activeChunks.Enqueue(chunk);
            _nextChunkStart = chunk.Bottom;


            // if this is the only one in the queue then its both the start point for new chunks
            // and the point after which we need to delete this chunk
            if (_activeChunks.Count == 1)
            {
                _topChunkEnd = _nextChunkStart;
            }
        }

        private LevelChunk BuildChunk(LevelChunk prefab)
        {
            return Instantiate(prefab, Vector3.zero, Quaternion.identity);
        }

        private void DestroyChunk(LevelChunk chunk)
        {
            Destroy(chunk.gameObject);

            if (_activeChunks.Count > 0)
            {
                _topChunkEnd = _activeChunks.Peek().Bottom;
            }
        }
    }
}