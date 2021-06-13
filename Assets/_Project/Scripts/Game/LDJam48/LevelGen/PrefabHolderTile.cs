using UnityEngine;
using UnityEngine.Tilemaps;

namespace LDJam48.LevelGen
{
        [CreateAssetMenu(menuName = "Custom/Tile/PrefabHolder")]
    public class PrefabHolderTile : TileBase
    {
        public GameObject Prefab;

        public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
        {
            base.GetTileData(position, tilemap, ref tileData);
            tileData.sprite = Prefab.GetComponentInChildren<SpriteRenderer>().sprite;
        }
    }
}