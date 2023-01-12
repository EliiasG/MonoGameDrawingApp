using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameDrawingApp.Ui.Themes
{
    public interface ITheme
    {
        Color BackgroundColor { get; }

        Color MenuBackgorundColor { get; }

        Color DefaultTextColor { get; }
        Color HoveringTextColor { get; }

        Color ButtonColor { get; }
        Color HoveringButtonColor { get; }
        Color SelectedButtonColor { get; }
    }
}
