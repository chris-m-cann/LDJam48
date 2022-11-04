using System;
using Unity.VisualScripting;
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
            Debug.Log("Swapping Colors in material");
            var matrix = new Matrix4x4();
            Color c0 = newPalette.GetColour(2);
            Color c1 = newPalette.GetColour(0);
            Color c2 = newPalette.GetColour(3);
            Color c3 = newPalette.GetColour(1);
            matrix.SetRow(0, ToVec4(c0));
            matrix.SetRow(1, ToVec4(c1));
            matrix.SetRow(2, ToVec4(c2));
            matrix.SetRow(3, ToVec4(c3));
            
            
            foreach (var mat in materials)
            {
                mat.SetMatrix("_ColorMatrix", matrix);
            }
        }
        
            
        private Vector4 ToVec4(Color c)
        {
            return new Vector4(c.r, c.g, c.b, c.a);
        }
    }

}