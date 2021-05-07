using System;
using UnityEngine;
using Util.Colour;
using Util.Var;
using Util.Var.Observe;

namespace LDJam48
{
    [ExecuteAlways]
    public class PaletteSwapper : MonoBehaviour
    {
        [SerializeField] private Material mat;
        [SerializeField] private ObservableColourPaletteVariable palette;

        private void Start()
        {
            SwapPalette(palette.Value);
        }

        private void OnEnable()
        {
            palette.OnValueChanged += SwapPalette;
        }

        private void OnDisable()
        {
            palette.OnValueChanged -= SwapPalette;
        }

        private void SwapPalette(ColourPalette newPalette)
        {
            mat.SetColor("Replacement1", newPalette.GetColour(2));
            mat.SetColor("Replacement2", newPalette.GetColour(1));
        }

    }
}