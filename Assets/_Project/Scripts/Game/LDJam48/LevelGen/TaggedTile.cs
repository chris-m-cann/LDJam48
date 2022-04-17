using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LDJam48.LevelGen
{
    [CreateAssetMenu(menuName = "Custom/Tile/Tagged")]
    public class TaggedTile : DesignTimeTile
    {
        public string TileTag;
        
        public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
        {
            base.GetTileData(position, tilemap, ref tileData);
            bool display = !Application.isPlaying || !HideAtRuntime;
            tileData.sprite = display ? Sprite : null;
            tileData.color = display ? Colour : Color.clear;
        }
    }
}