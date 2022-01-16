using System;
using LDJam48.LevelGen;
using Sirenix.Utilities;
using UnityEngine;
using Util;

namespace LDJam48.LevelGen
{
    [CreateAssetMenu(menuName = "Custom/Level/ChunkGenerators")]
    public class ChunkGenerators : ChunkGenerator
    {
        [SerializeField] private ChunkGenerator[] generators;
        [SerializeField] private bool moveToNextOnNull;
        

        [NonSerialized] private int _index = 0;
        [NonSerialized] private Func<GenerationData, LevelChunk> _generateFn;
        [NonSerialized] private GenerationData latest;
        public override void Init(GenerationData data)
        {
            _index = 0;
            latest = data;
            if (generators.IsNullOrEmpty())
            {
                _generateFn = it => null;
            }
            else
            {
                generators[0].Init(data);
                _generateFn = GenerateNextFn;
            }
        }

        private LevelChunk GenerateNextFn(GenerationData data)
        {
            return generators[_index].GenerateNext(data);
        }
        
        public override LevelChunk GenerateNext(GenerationData data)
        {
            latest = data;
            var r = _generateFn(data);
            if (moveToNextOnNull && r == null)
            {
                MoveToNext();
                r = _generateFn(data);
            }

            return r;
        }

        public bool MoveToNext()
        {
            ++_index;
            if (_index >= generators.Length)
            {
                _generateFn = it => null;
                return false;
            }
            else
            {
                generators[_index].Init(latest);
                return true;
            }
        }
    }
}