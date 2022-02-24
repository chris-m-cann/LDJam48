using LDJam48.LevelGen;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

namespace LDJam48
{
    public class LevelChunk : MonoBehaviour
    {
        public Vector2 Offset;
        public int Height;
        public int Width = 9;
        public int Intensity;
        public Tilemap GeometryTiles;
        public Tilemap EnemyTiles;
        public Tilemap PickupTiles;
        [Range(0, 1)]
        public float PickupTilesSpawnProbablity = 1f;

        public OnChunkBuilt[] OnBuildProcessors => GetComponentsInChildren<OnChunkBuilt>();

        public float Bottom => transform.position.y - Height;
    }
}