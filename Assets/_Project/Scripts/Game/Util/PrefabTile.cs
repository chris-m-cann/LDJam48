using UnityEngine;
using UnityEngine.Tilemaps;

namespace Util
{
    // mostly pinched from https://forum.unity.com/threads/prefab-tile.499903/
    [CreateAssetMenu(menuName = "Custom/Tile/PrefabTile")]
    public class PrefabTile : TileBase
    {
        [SerializeField] private GameObject prefab;

        public Matrix4x4 Mat;

        public GameObject Prefab => prefab;

        public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
        {
            base.GetTileData(position, tilemap, ref tileData);
            var sprite = prefab.GetComponentInChildren<SpriteRenderer>();

            Mat = tilemap.GetTransformMatrix(position);

            if (!Application.isPlaying) tileData.sprite = sprite.sprite;
            else tileData.sprite = null;
            tileData.gameObject = null; // prefab; dont do this as tilemap will instantiate without object pooling
        }

        public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
        {
            Debug.Log($"{position} rotation ITilemap = {tilemap.GetTransformMatrix(position).rotation.eulerAngles}");
            Debug.Log($"{position} position ITilemap = {tilemap.GetTransformMatrix(position).GetPosition()}");
            // StartUp(tilemap.GetTransformMatrix(position), go);
            
            // var worldPos = tilemap.CellToWorld(cell) + new Vector3(.5f, .5f);
            // Debug.Log($"{cell} spike rot = {_tilemap.GetTransformMatrix(cell).rotation.eulerAngles}");
            // var go = Instantiate(holder.Prefab, worldPos, _tilemap.GetTransformMatrix(cell).rotation);

            return base.StartUp(position, tilemap, go);
        }

        public void StartUp(Matrix4x4 tilemapTransform, GameObject go)
        {
            if (go != null)
            {
                go.transform.rotation = tilemapTransform.rotation;
            }
        }
    }
}