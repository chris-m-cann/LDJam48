using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LDJam48.LevelGen
{
    [CreateAssetMenu(menuName="Custom/Tile/ChunkRuleTile")]
    public class ChunkRuleTile : RuleTile<ChunkRuleTile.Neighbor>
    {
        [SerializeField] private TileBase rule1Tile;
        [SerializeField] private TileBase rule2Tile;
        [SerializeField] private TileBase rule3Tile;
        [SerializeField] private TileBase rule4Tile;
        
        public class Neighbor : RuleTile.TilingRule.Neighbor {
            // 0, 1, 2 is using in RuleTile.TilingRule.Neighbor
            public const int IsRule3Tile = 3;
            public const int IsRule4Tile = 4;
            public const int IsRule5Tile = 5;
            public const int IsRule6Tile = 6;
        }
        public override bool RuleMatch(int neighbor, TileBase tile) {
            switch (neighbor) {
                case Neighbor.IsRule3Tile: return rule1Tile != null && tile == rule1Tile;
                case Neighbor.IsRule4Tile: return rule2Tile != null && tile == rule2Tile;
                case Neighbor.IsRule5Tile: return rule3Tile != null && tile == rule3Tile;
                case Neighbor.IsRule6Tile: return rule4Tile != null && tile == rule4Tile;
            }
            return base.RuleMatch(neighbor, tile);
        }

        [Button("Order rules")]
        [ContextMenu("Order Items")]
        public void OrderRules()
        {
            m_TilingRules.Sort(CompareRules);
        }
        
        public int CompareRules(TilingRule lhs, TilingRule rhs)
        {
            var lhsContainsCustomRules = lhs.m_Neighbors.Exists(rule => rule >= Neighbor.IsRule3Tile);
            var rhsContainsCustomRules = rhs.m_Neighbors.Exists(rule => rule >= Neighbor.IsRule3Tile);

            int customRulesComp = rhsContainsCustomRules.CompareTo(lhsContainsCustomRules);

            if (customRulesComp != 0)
            {
                return customRulesComp;
            }
            
            return rhs.m_Neighbors.Count.CompareTo(lhs.m_Neighbors.Count);
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