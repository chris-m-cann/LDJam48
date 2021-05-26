using UnityEngine;

namespace LDJam48
{
    public class LevelChunk : MonoBehaviour
    {
        public Vector2 Offset;
        public float Height;
        public int Intensity;

        public float Bottom => transform.position.y - Height;
    }
}