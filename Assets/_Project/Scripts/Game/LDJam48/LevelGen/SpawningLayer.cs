using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;
using Util;
using Util.ObjPool;

namespace LDJam48.LevelGen
{
    [RequireComponent(typeof(Tilemap))]
    public class SpawningLayer : OnChunkBuilt
    {
        [SerializeField] private bool disableOnComplete;
        
        protected Tilemap _tilemap;

        private void Awake()
        {
            _tilemap = GetComponent<Tilemap>();
        }

        
        public override IEnumerator OnBuilt(Parameters spawn)
        {
            yield return null;
            try
            {
                SpawnImpl(spawn);
            }
            finally
            {
                gameObject.SetActive(!disableOnComplete);
            }
        }

        protected void SpawnImpl(Parameters spawn)
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
                    if (tile is PrefabTile holder)
                    {
                        var worldPos = _tilemap.CellToWorld(cell) + new Vector3(.5f, .5f);
                        var mat = _tilemap.GetTransformMatrix(cell);

                        var zrot = mat.rotation.eulerAngles.z;
                        var instance = InstantiateEx.Create(holder.Prefab, worldPos, Quaternion.Euler(0, 0, zrot));
                        
                        var flipX = Mathf.Approximately(mat.m00, -1);
                        var flipY = Mathf.Approximately(mat.m11, -1);
                        if (flipX || flipY)
                        {
                            if (instance.TryGetComponent(out SpriteRenderer sprite))
                            {
                                sprite.flipX = flipX;
                                //sprite.flipY = flipY;
                            }
                        }
                    }
                }
            }
        }
    }
}