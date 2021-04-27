using UnityEngine;

namespace Util.Colour
{
    [RequireComponent(typeof(Camera))]
    public class CameraBackgroundColourSwitcher : ColourSwitcher
    {
        private Camera _camera;

        protected override void SetColour(Color colour)
        {
            _camera = GetComponent<Camera>();
            _camera.backgroundColor = colour;
        }
    }
}