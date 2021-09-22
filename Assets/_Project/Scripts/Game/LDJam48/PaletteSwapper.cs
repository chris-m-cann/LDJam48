using System;
using UnityEngine;
using Util;
using Util.Colour;
using Util.Var.Observe;

namespace LDJam48
{
    [ExecuteAlways]
    public class PaletteSwapper : MonoBehaviour
    {
        [SerializeField] private MaterialColourReplacements[] swaps;
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
            foreach (var swap in swaps)
            {
                swap.ReplaceColours(newPalette);
            }
        }

    }
}