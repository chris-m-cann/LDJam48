using UnityEngine;
using Util.Colour;
using Util.Var.Observe;

namespace Util
{
    [ExecuteAlways, ImageEffectAllowedInSceneView]
    public class ImageEffectBuiltInRenderer : MonoBehaviour
    {
        [SerializeField] private ObservableColourPaletteVariable palette;
        
        [SerializeField] private Material mat;

        private Matrix4x4 _matrix = new Matrix4x4();
        
        private void OnRenderImage(RenderTexture src, RenderTexture dest)
        {
            SetColorMatrix();
            Graphics.Blit(src, dest, mat);
        }

        private void SetColorMatrix()
        {
            ReplaceColours(palette.Value);
        }
        
        public void ReplaceColours(ColourPalette newPalette)
        {
            Color c0 = newPalette.GetColour(2);
            Color c1 = newPalette.GetColour(0);
            Color c2 = newPalette.GetColour(3);
            Color c3 = newPalette.GetColour(1);
            _matrix.SetRow(0, ToVec4(c0));
            _matrix.SetRow(1, ToVec4(c1));
            _matrix.SetRow(2, ToVec4(c2));
            _matrix.SetRow(3, ToVec4(c3));
            
            
            mat.SetMatrix("_ColorMatrix", _matrix);
        }
        
            
        private Vector4 ToVec4(Color c)
        {
            return new Vector4(c.r, c.g, c.b, c.a);
        }
    }
}