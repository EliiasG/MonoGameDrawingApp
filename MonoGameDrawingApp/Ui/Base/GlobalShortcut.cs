using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace MonoGameDrawingApp.Ui.Base
{
    public class GlobalShortcut
    {
        public GlobalShortcut(IEnumerable<Keys> activationKeys, Action run)
        {
            ActivationKeys = activationKeys;
            Run = run;
        }

        public IEnumerable<Keys> ActivationKeys { get; }

        public Action Run { get; }
    }
}
