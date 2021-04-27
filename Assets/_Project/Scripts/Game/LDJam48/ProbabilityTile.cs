using UnityEngine;
using UnityEngine.Tilemaps;
using Util;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace LDJam48
{
    [CreateAssetMenu(menuName = "Custom/Tile/Probability")]
    public class ProbabilityTile : TileBase
    {
        [SerializeField] private Sprite sprite;
        [SerializeField] private Color colour;
        public SpawnDetails[] Spawnables;




        public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
        {
            base.GetTileData(position, tilemap, ref tileData);
            tileData.sprite = sprite;
            tileData.color = colour;
        }

        // [MenuItem("Assets/Create/Custom/Tile/Probability")]
        // public static void CreateTile()
        // {
        //     AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<ProbabilityTile>()));
        // }
    }
}
