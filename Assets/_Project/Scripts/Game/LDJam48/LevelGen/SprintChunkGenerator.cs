using System;
using UnityEngine;
using Util;
using Random = UnityEngine.Random;

namespace LDJam48.LevelGen
{
    [CreateAssetMenu(menuName = "Custom/Level/SprintChunkGenerator")]
    public class SprintChunkGenerator : ChunkGenerator
    {
        public IntensityBasedChunkGenerator[] ChunkGenerators;
        public ChunkGenerator RestGenerator;
        public int NumChunks;
        public int NumChunksFuzz;

        public float IntensityStart;
        public float IntensityEnd;
        // todo(chris) add curve type

        [NonSerialized] private int _chunkIdx;
        [NonSerialized] private Curve _curve;
        [NonSerialized] private int _chunksThisSprint;
        [NonSerialized] private IntensityBasedChunkGenerator _currentGenerator;
        public override LevelChunk GenerateNext()
        {
            // if not started sprint yet the set up
            if (_chunkIdx == 0) Init();

            // if generated all the chunks for this sprint then add on the end of sprint rest
            if (_chunkIdx == _chunksThisSprint)
            {
                Reset();
                return RestGenerator.GenerateNext();
            }


            // -1 here so last index is == IntensityEnd
            var intensity = _curve.GetPoint(_chunkIdx, _chunksThisSprint - 1);
            _currentGenerator.Intensity = intensity;
            var chunk = _currentGenerator.GenerateNext();

            ++_chunkIdx;
            return chunk;
        }

        private void Init()
        {
            _curve = new Curve(IntensityStart, IntensityEnd, t => t);
            _currentGenerator = ChunkGenerators.RandomElement();
            _chunksThisSprint = Random.Range(NumChunks - NumChunksFuzz, NumChunks + NumChunksFuzz + 1);
        }

        private void Reset()
        {
            _chunkIdx = 0;
        }
    }
}