using System;
using UnityEngine;
using Util.Variable;

namespace Util.Colour
{
    [ExecuteAlways]
    public abstract class ColourSwitcher : MonoBehaviour
    {
        [SerializeField] private ObservableColourPaletteVariable colours;
        [SerializeField] private int colourIndex;

        [Range(0, 1)]
        [SerializeField] float alphaOverride = 1;

        protected abstract void SetColour(Color colour);

        private void OnEnable()
        {
            SetUpColour();
        }

        private void OnDisable()
        {
            if (colours == null) return;
            colours.Value.OnChange -= SetUpColour;

            if (colours.Value == null) return;
            colours.OnValueChanged -= SetUpColour;
        }

        private void OnValidate()
        {
            SetColourFromPalette();
        }

        private void SetColourFromPalette()
        {
            if (colours == null) return;
            if (colours.Value == null) return;
            var color = colours.Value.GetColour(colourIndex);
            color.a = alphaOverride;

            SetColour(color);
        }

        private void SetUpColour(ColourPalette palette)
        {
            SetColourFromPalette();

            colours.Value.OnChange -= SetUpColour;
            colours.OnValueChanged -= SetUpColour;
            colours.OnValueChanged += SetUpColour;
            colours.Value.OnChange += SetUpColour;
        }

        private void SetUpColour() => SetUpColour(colours.Value);



        public void ChangeColour(int newColour)
        {
            colourIndex = newColour;
            SetUpColour();
        }
    }
}