using System;
using UnityEngine;
using Util.Variable;

namespace Util.Colour
{
    [ExecuteAlways]
    public abstract class ColourSwitcher : MonoBehaviour
    {
        [SerializeField] private ColourPaletteVariable colours;
        [SerializeField] private int colourIndex;

        [Range(0, 1)]
        [SerializeField] float alphaOverride = 1;

        protected abstract void SetColour(Color colour);

        private void Awake()
        {
            SetUpColour();
        }

        private void OnDisable()
        {
            colours.Value.OnChange -= SetUpColour;
        }

        // works great in editor but breaks at runtime
        // private void OnValidate()
        // {
        //     SetUpColour();
        // }

        private void SetUpColour()
        {
            if (colours == null) return;
            if (colours.Value == null) return;
            var color = colours.Value.GetColour(colourIndex);
            color.a = alphaOverride;

            SetColour(color);

            colours.Value.OnChange -= SetUpColour;
            colours.Value.OnChange += SetUpColour;
        }



        public void ChangeColour(int newColour)
        {
            colourIndex = newColour;
            SetUpColour();
        }
    }
}