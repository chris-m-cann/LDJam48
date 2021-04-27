using System;
using UnityEngine;
using Util.Variable;

namespace LDJam48
{
    [ExecuteAlways]
    public class PaletteSwapper : MonoBehaviour
    {
        [SerializeField] private Material mat;
        [SerializeField] private ColourPaletteVariable palette;

        private void Start()
        {
            SwapPalette();
        }

        private void OnEnable()
        {
            palette.Value.OnChange += SwapPalette;
        }

        private void OnDisable()
        {
            palette.Value.OnChange -= SwapPalette;
        }

        private void SwapPalette()
        {
            mat.SetColor("Replacement1", palette.Value.GetColour(2));
            mat.SetColor("Replacement2", palette.Value.GetColour(1));
        }

    }
}