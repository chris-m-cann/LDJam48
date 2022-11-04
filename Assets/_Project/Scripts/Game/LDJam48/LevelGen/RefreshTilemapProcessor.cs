using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using Util;

namespace LDJam48.LevelGen
{
    [RequireComponent(typeof(Tilemap))]
    public class RefreshTilemapProcessor : OnChunkBuilt
    {
        private Tilemap _tilemap;
        private void Awake()
        {
            _tilemap = GetComponent<Tilemap>();
            
        }


        [ContextMenu("RefreshAllTiles")]
        public void RefreshAllTiles()
        {
            _tilemap.RefreshAllTiles();
        }

        public override IEnumerator OnBuilt(Parameters spawn)
        {
            _tilemap.CompressBounds();

            BoundsInt bounds = _tilemap.cellBounds;
            int xmin = bounds.xMin;
            int xmax = bounds.xMax;
            int ymin = bounds.yMin;
            int ymax = bounds.yMax - 1; // want inclusive max in this case

            for (int x = xmin; x < xmax; ++x)
            {
                _tilemap.RefreshTile(new Vector3Int(x, ymin, 0));
                _tilemap.RefreshTile(new Vector3Int(x, ymax, 0));
            }
            
            
            yield break;
        }
    }
}