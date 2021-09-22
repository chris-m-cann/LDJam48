using System;
using UnityEngine;

namespace Util.Colour
{
    [CreateAssetMenu(menuName = "Custom/MaterialColourReplacements")]
    public class MaterialColourReplacements : ScriptableObject
    {
        [SerializeField] private Material[] materials;
        [Tooltip("A list of Shader properties to replace with the colour in the new palette at the specified index")]
        [SerializeField] private Pair<String, int>[] replacements;

        public void ReplaceColours(ColourPalette newPalette)
        {
            foreach (var mat in materials)
            {
                foreach (var replacement in replacements)
                {
                    mat.SetColor(replacement.First, newPalette.GetColour(replacement.Second));
                }
            }
        }
    }
}