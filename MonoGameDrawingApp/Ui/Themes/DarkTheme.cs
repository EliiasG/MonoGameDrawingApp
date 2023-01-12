using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameDrawingApp.Ui.Themes
{
    public class DarkTheme : ITheme
    {
        public Color BackgroundColor => new Color(45, 54, 64);

        public Color MenuBackgorundColor => new Color(37, 42, 48);

        public Color DefaultTextColor => new Color(132, 146, 163);

        public Color HoveringTextColor => new Color(218, 222, 227);

        public Color ButtonColor => new Color(66, 75, 87);

        public Color HoveringButtonColor => new Color(77, 89, 105);

        public Color SelectedButtonColor => new Color(52, 59, 69);
    }
}
