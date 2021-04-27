using TMPro;
using UnityEngine;
using Util.Variable;

namespace Util.Colour
{
    [RequireComponent(typeof(TMP_Text))]
    public class FontColourSwitcher : ColourSwitcher
    {
        protected override void SetColour(Color colour)
        {
            var text = GetComponent<TMP_Text>();

            if (text == null) return;

            text.color = colour;
        }
    }
}