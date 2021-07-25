using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LDJam48.LevelGen
{
    [RequireComponent(typeof(Tilemap))]
    public class SpawningLayer : MonoBehaviour
    {
        public struct SpawnParameters
        {
            public LevelChunk Chunk;
            public Vector3 ChunkStartPos;
            // todo(chris) add in stuff like intensity requested or whatever
        }

        protected Tilemap _tilemap;

        private void Awake()
        {
            _tilemap = GetComponent<Tilemap>();
        }

        public IEnumerator Spawn(SpawnParameters spawn)
        {
            yield return null;
            try
            {
                SpawnImpl(spawn);
            }
            finally
            {
                gameObject.SetActive(false);
            }
        }

        protected virtual void SpawnImpl(SpawnParameters spawn)
        {
            var height = spawn.Chunk.Height;
            var width = spawn.Chunk.Width;
            var startPos = spawn.ChunkStartPos + new Vector3(-4, -.5f);
            var startCell = _tilemap.WorldToCell(startPos);
            var cell = new Vector3Int(startCell.x, startCell.y, startCell.z);

            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    cell.x = x + startCell.x;
                    cell.y = startCell.y - y;
                    var tile = _tilemap.GetTile(cell);
                    if (tile is PrefabHolderTile holder)
                    {
                        var worldPos = _tilemap.CellToWorld(cell) + new Vector3(.5f, .5f);
                        Instantiate(holder.Prefab, worldPos,
                            _tilemap.GetTransformMatrix(cell).rotation);
                    }
                }
            }
        }
    }
}