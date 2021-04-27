using System;
using UnityEngine;
using Util;
using Util.Colour;
using Util.Variable;

namespace LDJam48
{
    public class RandomPalette : MonoBehaviour
    {
        [SerializeField] private ColourPaletteVariable activePalette;
        [SerializeField] private ColourPalette[] palettes;
        [SerializeField] private bool randomizeOnStart;


        private void Start()
        {
            if (randomizeOnStart)
            {
                RandomizePalette();
            }
        }

        public void RandomizePalette()
        {
            var palette = palettes.RandomElement();

            activePalette.Value.Overwrite(palette);
        }
    }
}