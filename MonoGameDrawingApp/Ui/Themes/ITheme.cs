﻿using Microsoft.Xna.Framework;

namespace MonoGameDrawingApp.Ui.Themes
{
    public interface ITheme
    {
        Color BackgroundColor { get; }

        Color MenuBackgorundColor { get; }
        Color SecondaryMenuBackgroundColor { get; }

        Color DefaultTextColor { get; }
        Color HoveringTextColor { get; }

        Color ButtonColor { get; }
        Color HoveringButtonColor { get; }
        Color SelectedButtonColor { get; }
    }
}
