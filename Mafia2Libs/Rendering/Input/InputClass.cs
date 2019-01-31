using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Rendering.Input
{
    public class InputClass
    {
        private Dictionary<Keys, bool> InputKeys = new Dictionary<Keys, bool>();
        private Dictionary<MouseButtons, bool> InputMouse = new Dictionary<MouseButtons, bool>();

        internal void Init()
        {
            foreach (Keys key in Enum.GetValues(typeof(Keys)))
                InputKeys[key] = false;

            foreach (MouseButtons key in Enum.GetValues(typeof(MouseButtons)))
                InputMouse[key] = false;

        }
        internal bool IsKeyDown(Keys key)
        {
            return InputKeys[key];
        }
        internal void KeyDown(Keys key)
        {
            InputKeys[key] = true;
        }
        internal void KeyUp(Keys key)
        {
            InputKeys[key] = false;
        }
        internal bool IsButtonDown(MouseButtons button)
        {
            return InputMouse[button];
        }
        internal void ButtonDown(MouseButtons button)
        {
            InputMouse[button] = true;
        }
        internal void ButtonUp(MouseButtons button)
        {
            InputMouse[button] = false;
        }


    }
}
