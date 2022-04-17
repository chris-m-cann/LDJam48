using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LDJam48.LevelGen
{
    [RequireComponent(typeof(Tilemap))]
    public class RefreshTilemapOnAwake : MonoBehaviour
    {
        [SerializeField] private Bounds[] tilesToRefresh;
        
        
        private Tilemap _tilemap;

        // dont call it twice!! it wipes the rotation from prefab tiles rotation matrix
        private void Awake()
        {
            _tilemap = GetComponent<Tilemap>();
            // _tilemap.RefreshAllTiles();
            
        }

        private void OnEnable()
        {
            _tilemap.RefreshAllTiles();
            // RefreshTiles();
        }

        [ContextMenu("RefreshAllTiles")]
        public void RefreshAllTiles()
        {
            _tilemap.RefreshAllTiles();
        }

        private void RefreshTiles()
        {
            foreach (var bounds in tilesToRefresh)
            {
                Vector3Int minCell = _tilemap.WorldToCell(transform.position + bounds.min);
                Vector3Int maxCell = _tilemap.WorldToCell(transform.position + bounds.max);
                Vector3 cellSize = _tilemap.cellSize;
                var cells = new Vector3Int(
                    Mathf.RoundToInt(Mathf.Round(2 * bounds.extents.x) / cellSize.x),
                    Mathf.Min(1, Mathf.RoundToInt(Mathf.Round(2 * bounds.extents.y) / cellSize.y)),
                    Mathf.RoundToInt(Mathf.Round(2 * bounds.extents.z) / cellSize.z)
                    );

                for (int y = minCell.y; y <= maxCell.y; y++)
                {
                    for (int x = minCell.x; x <= maxCell.x; x++)
                    {
                        var pos = new Vector3Int(
                            x,
                            y,
                            0
                        );

                        _tilemap.RefreshTile(pos);
                    }
                }
            }
        }
    }
}