using System;
using System.Collections;
using Unity.Mathematics;
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

                        var flipX = Mathf.Approximately(mat.m00, -1);
                        var flipY = Mathf.Approximately(mat.m11, -1);

                        var eulers = mat.rotation.eulerAngles;
                        var zrot = eulers.z;
                        
                        
                        GameObject instance;
                        
                        
                        
                        instance = InstantiateEx.Create(holder.Prefab, worldPos, mat.rotation);

                        // wierd bug i couldnt figure out on mobile. x flipped tiles had eulers ~= Vector3(309.39f, 318.79f, 256.99f)
                        // so having to limit to either rot or flip. not both
                        // if (flipX)
                        // {
                        //     if (instance.TryGetComponent(out SpriteRenderer sprite))
                        //     {
                        //         sprite.flipX = flipX;
                        //     }
                        // }
                        //
                        // if (flipY)
                        // {
                        //     if (instance.TryGetComponent(out SpriteRenderer sprite))
                        //     {
                        //         sprite.flipY = flipY;
                        //     }
                        // }
                        
                        // Debug.LogError($"spawning {instance.name}: rot={eulers}/{zrot}, flip=({flipX}, {flipY})");
                        
                    }
                }
            }
        }
    }
}