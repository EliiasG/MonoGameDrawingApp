using Microsoft.Xna.Framework;

namespace MonoGameDrawingApp.Ui.Base.Themes
{
    public class DarkTheme : ITheme
    {
        public Color BackgroundColor => new(57, 69, 82);

        public Color MenuBackgorundColor => new(30, 35, 41);

        public Color DefaultTextColor => new(208, 226, 247);

        public Color HoveringTextColor => new(156, 170, 186);

        public Color ButtonColor => new(88, 103, 122);

        public Color HoveringButtonColor => new(77, 89, 105);

        public Color SelectedButtonColor => new(52, 59, 69);

        public Color SecondaryMenuBackgroundColor => new(58, 79, 102);

        public Color EditingTextColor => new(232, 252, 239);

        public Color ScrollbarColor => new(44, 51, 59);
    }
}
