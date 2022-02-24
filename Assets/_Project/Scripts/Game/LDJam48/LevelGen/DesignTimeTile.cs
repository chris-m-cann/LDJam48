using UnityEngine;
using UnityEngine.Tilemaps;

namespace LDJam48.LevelGen
{
    public abstract class DesignTimeTile: TileBase
    {
        public Sprite Sprite;
        public Color Colour;
        public bool HideAtRuntime = true;

        public override void RefreshTile(Vector3Int position, ITilemap tilemap) 
        {
            tilemap.RefreshTile(position);
        }
    }
}