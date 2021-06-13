using UnityEngine;
using UnityEngine.Tilemaps;

namespace LDJam48
{
    public class LevelChunk : MonoBehaviour
    {
        public Vector2 Offset;
        public int Height;
        public int Intensity;
        public Tilemap GeometryTiles;
        public Tilemap SpawnTiles;

        public float Bottom => transform.position.y - Height;
    }
}