using UnityEngine;
using UnityEngine.Tilemaps;

namespace Util
{
    // mostly pinched from https://forum.unity.com/threads/prefab-tile.499903/
    [CreateAssetMenu(menuName = "Custom/Tile/PrefabTile")]
    public class PrefabTile : TileBase
    {
        [SerializeField] private GameObject prefab;


        public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
        {
            base.GetTileData(position, tilemap, ref tileData);
            var sprite = prefab.GetComponentInChildren<SpriteRenderer>();

            if (!Application.isPlaying) tileData.sprite = sprite.sprite;
            else tileData.sprite = null;
            tileData.gameObject = prefab;
        }

        public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
        {
            if (go != null)
            {
                var transform = tilemap.GetTransformMatrix(position);
                go.transform.rotation = transform.rotation;
            }

            return base.StartUp(position, tilemap, go);
        }

        // OP that I pinched the Prefab part of this from has this but not sure why its needed so commented out for now
        // public override bool GetTileAnimationData(Vector3Int location, ITilemap tileMap, ref TileAnimationData tileAnimationData)
        // {
        //     tileAnimationData.animatedSprites = new Sprite[] { null};
        //     tileAnimationData.animationSpeed = 0;
        //     tileAnimationData.animationStartTime = 0;
        //     return true;
        // }

    }
}