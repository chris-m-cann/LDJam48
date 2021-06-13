using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Util;

[CreateAssetMenu(menuName = "Custom/Tile/RuntimeRandomRuleTile")]
public class RuntimeRandomRuleTile : RuleTile<RuntimeRandomRuleTile.Neighbor>
{
    public TilingRule CurrentRule;

    public class Neighbor : RuleTile.TilingRule.Neighbor
    {
        public const int Null = 3;
        public const int NotNull = 4;
    }

    public override bool RuleMatch(int neighbor, TileBase tile)
    {
        return base.RuleMatch(neighbor, tile);
    }

    public override bool RuleMatches(TilingRule rule, Vector3Int position, ITilemap tilemap, ref Matrix4x4 transform)
    {
        if (base.RuleMatches(rule, position, tilemap, ref transform))
        {
            CurrentRule = rule;
            return true;
        }
        else
        {
            return false;
        }
    }

    public override void RefreshTile(Vector3Int position, ITilemap tilemap)
    {
        Debug.Log("Refreshing tile");
        base.RefreshTile(position, tilemap);
    }

    public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject instantiatedGameObject)
    {
        Debug.Log("StartUp called");
        return base.StartUp(position, tilemap, instantiatedGameObject);
    }


    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        Debug.Log("GetTileData called");

        base.GetTileData(position, tilemap, ref tileData);

        if (CurrentRule != null && CurrentRule.m_Output == TilingRuleOutput.OutputSprite.Random)
        {
            // int index = Mathf.Clamp(
            //     Mathf.FloorToInt(GetPerlinValue(position, CurrentRule.m_PerlinScale, 100000f) * CurrentRule.m_Sprites.Length), 0,
            //     CurrentRule.m_Sprites.Length - 1);
            // tileData.sprite = CurrentRule.m_Sprites[index];
            tileData.sprite = CurrentRule.m_Sprites.RandomElement();
        }
    }

    public void Randomize()
    {
    }
}