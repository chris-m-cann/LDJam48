using UnityEngine;
using UnityEngine.Tilemaps;

namespace LDJam48.LevelGen
{
    [CreateAssetMenu(menuName="Custom/Tile/ChunkRuleTile")]
    public class ChunkRuleTile : RuleTile<ChunkRuleTile.Neighbor>
    {
        public class Neighbor : RuleTile.TilingRule.Neighbor {
            // 0, 1, 2 is using in RuleTile.TilingRule.Neighbor
            public const int MyRule1 = 3;
            public const int MyRule2 = 4;
        }
        public override bool RuleMatch(int neighbor, TileBase tile) {
            switch (neighbor) {
                case Neighbor.MyRule1: return IsTaggedTile(tile);
                case Neighbor.MyRule2: return true;
            }
            return base.RuleMatch(neighbor, tile);
        }

        private bool IsTaggedTile(TileBase tile)
        {
            if (tile is TaggedTile tagged)
            {
                return tagged.TileTag == "End";
            }

            return false;
        }
    }
}