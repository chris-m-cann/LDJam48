using System.Collections;
using UnityEngine;

namespace LDJam48.LevelGen
{
    public abstract class OnChunkBuilt : MonoBehaviour
    {
        public struct Parameters
        {
            public LevelChunk Chunk;
            public Vector3 ChunkStartPos;
            // todo(chris) add in stuff like intensity requested or whatever
        }

        public abstract IEnumerator OnBuilt(Parameters spawn);
    }
}