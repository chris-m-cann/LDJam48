using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Util
{
    // mostly pinched from https://forum.unity.com/threads/prefab-tile.499903/
    [CreateAssetMenu(menuName = "Custom/Tile/PrefabTile")]
    public class PrefabTile : TileBase
    {
        [SerializeField] private GameObject prefab;

        public GameObject Prefab => prefab;

        public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
        {
            base.GetTileData(position, tilemap, ref tileData);
            var sprite = prefab.GetComponentInChildren<SpriteRenderer>();

            if (!Application.isPlaying)
            {
                tileData.sprite = sprite.sprite;
                if (sprite.flipX)
                {
                    tileData.color = Color.cyan;
                } else if (sprite.flipY)
                {
                    tileData.color = Color.magenta;
                }
            }
            else
            {
                tileData.sprite = null;
            }
            
            
            tileData.gameObject = null; // prefab; dont do this as tilemap will instantiate without object pooling
        }
    }
}