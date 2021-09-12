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
        [Serializable]
        public struct SwapParams
        {
            public Material Material;
            public Pair<string, int>[] Replacements;
        }

        [SerializeField] private SwapParams[] swaps;
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
                foreach (var replacement in swap.Replacements)
                {
                    swap.Material.SetColor(replacement.First, newPalette.GetColour(replacement.Second));
                }
            }
        }

    }
}