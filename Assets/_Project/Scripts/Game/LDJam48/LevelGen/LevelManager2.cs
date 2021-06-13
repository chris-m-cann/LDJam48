using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.Tilemaps;
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

        private IEnumerator SpawnThings(LevelChunk chunk, Vector3 p)
        {
            chunk.SpawnTiles.gameObject.SetActive(true);

            var startPos = p + new Vector3(-4, -.5f);
            var startCell = chunk.SpawnTiles.WorldToCell(startPos);
            var width = 9;
            var height = chunk.Height;
            var cell = new Vector3Int(startCell.x, startCell.y, startCell.z);

            Debug.DrawLine(startPos, startPos + new Vector3(width, -height), Color.red, 1);
            Debug.DrawLine(startPos + Vector3.right * width, startPos + new Vector3(0, -height), Color.red, 1);

            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    cell.x = x + startCell.x;
                    cell.y = startCell.y - y;
                    var tile = chunk.SpawnTiles.GetTile(cell);
                    if (tile is PrefabHolderTile holder)
                    {
                        var worldPos = chunk.SpawnTiles.CellToWorld(cell) + new Vector3(.5f, .5f);
                        Instantiate(holder.Prefab, worldPos,
                        chunk.SpawnTiles.GetTransformMatrix(cell).rotation);
                    }
                }
            }

            chunk.SpawnTiles.gameObject.SetActive(false);
            yield break;
        }

        private LevelChunk BuildChunk(LevelChunk prefab)
        {
            var chunk = Instantiate(prefab, Vector3.zero, Quaternion.identity);

            return chunk;
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


            StartCoroutine(SpawnThings(chunk, p));
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