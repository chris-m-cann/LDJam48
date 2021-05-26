using UnityEngine;

namespace LDJam48.LevelGen
{
    public class LevelBuilder : MonoBehaviour
    {
        [SerializeField] private LevelAssembler assembler;
        [SerializeField] private SprintGenerator sprintGenerator;
        [SerializeField] private LevelChunk gapChunk;
        [SerializeField] private int chunksPerSprint = 3;
        [SerializeField] private float curveStart = 1;
        [SerializeField] private float curveEnd = 15;


        public void BuildSprint()
        {
            BuildSprint(chunksPerSprint, new Curve(curveStart, curveEnd, t => t * t));
        }

        public void BuildSprint(int numChunks, Curve curve)
        {
            var sprint = sprintGenerator.GenerateSprint(numChunks, curve);

            foreach (var chunk in sprint)
            {
                assembler.AppendChunk(BuildChunk(chunk));
            }

            assembler.AppendChunk(BuildChunk(gapChunk));
        }

        private LevelChunk BuildChunk(LevelChunk prefab)
        {
            return Instantiate(prefab, Vector3.zero, Quaternion.identity);
        }

    }
}