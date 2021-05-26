using System;
using UnityEngine;

namespace LDJam48.LevelGen
{
    public class LevelAssembler : MonoBehaviour
    {

        [SerializeField] private float startPoint;

        private float _nextChunkStart;

        private void Awake()
        {
            _nextChunkStart = startPoint;
        }

        public void AppendChunk(LevelChunk chunk)
        {
            var t = chunk.transform;
            var p = t.position;
            p.y = _nextChunkStart;
            t.position = p;

            _nextChunkStart = chunk.Bottom;
        }
    }
}