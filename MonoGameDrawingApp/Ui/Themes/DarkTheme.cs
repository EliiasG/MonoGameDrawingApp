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
        public Color BackgroundColor => new Color(57, 69, 82);

        public Color MenuBackgorundColor => new Color(30, 35, 41);

        public Color DefaultTextColor => new Color(208, 226, 247);

        public Color HoveringTextColor => new Color(156, 170, 186);

        public Color ButtonColor => new Color(88, 103, 122);

        public Color HoveringButtonColor => new Color(77, 89, 105);

        public Color SelectedButtonColor => new Color(52, 59, 69);
    }
}
