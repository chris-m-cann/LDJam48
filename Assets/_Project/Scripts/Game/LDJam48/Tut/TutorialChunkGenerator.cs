using System;
using LDJam48.LevelGen;
using Unity.VisualScripting;
using UnityEngine;

namespace LDJam48.Tut
{
    [CreateAssetMenu(menuName = "Custom/Level/TutorialGenerator")]
    public class TutorialChunkGenerator : ChunkGenerator
    {
        [SerializeField] private ChunkGenerator tutorialChunks;
        [SerializeField] private ChunkGenerator runChunks;
        [SerializeField] private TutorialSaveableSO tutorialSaveData;

        private Func<GenerationData, LevelChunk> _generator;
        public override LevelChunk GenerateNext(GenerationData data)
        {
            return _generator(data);
        }

        public override void Init(GenerationData data)
        {
            tutorialChunks.Init(data);
            runChunks.Init(data);

            _generator = GenerateNextTutorial;
        }

        private LevelChunk GenerateNextTutorial(GenerationData data)
        {
            if (tutorialSaveData.Data.TutorialRequired)
            {
                return tutorialChunks.GenerateNext(data);   
            }
            else
            {
                _generator = GenerateNextRun;
                return GenerateNextRun(data);
            }
        }
        
        private LevelChunk GenerateNextRun(GenerationData data)
        {
            return runChunks.GenerateNext(data);
        }
    }
}