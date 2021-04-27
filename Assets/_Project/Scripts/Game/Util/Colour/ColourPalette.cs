using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Util.Colour
{
    [CreateAssetMenu(menuName = "Custom/ColourPallete")]
    public class ColourPalette : ScriptableObject
    {
        [SerializeField] private Color[] colours;
        public Color NotFound;

#if UNITY_EDITOR
        [Tooltip("Where I found the palette")]
        [SerializeField] private string url;
#endif

        public event Action OnChange;

        public Color GetColour(int index)
        {
            if (index < 0 || index >= colours.Length) return NotFound;

            return colours[index];
        }

        public void Overwrite(ColourPalette other)
        {
            colours = other.colours;
            NotFound = other.NotFound;
            OnValidate();
        }

        private void OnValidate()
        {
            OnChange?.Invoke();
        }
    }
}