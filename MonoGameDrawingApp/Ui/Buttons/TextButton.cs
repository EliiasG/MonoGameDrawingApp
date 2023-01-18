using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameDrawingApp.Ui.Themes;

namespace MonoGameDrawingApp.Ui.Buttons
{
    public class TextButton : IUiElement
    {
        public readonly Button Button;
        public readonly string Text;

        private readonly UiEnvironment _environment;

        private readonly TextView _textView;
        private readonly ColorModifier _colorModifier;

        public TextButton(UiEnvironment environment, string text)
        {
            Text = text;
            _environment = environment;
            _textView = new TextView(environment, text);
            _colorModifier = new ColorModifier(environment, _textView, _theme.DefaultTextColor);
            Button = new Button(environment, _colorModifier);
        }


        public bool Changed => Button.Changed;

        public int RequiredWidth => Button.RequiredWidth;

        public int RequiredHeight => Button.RequiredHeight;

        public UiEnvironment Environment => _environment;

        private ITheme _theme => _environment.Theme;

        public Texture2D Render(Graphics graphics, int width, int height)
        {
            return Button.Render(graphics, width, height);
        }

        public void Update(Vector2 position, int width, int height)
        {
            _colorModifier.Color = Button.ContainsMouse ? _theme.HoveringTextColor : _theme.DefaultTextColor;

            if(Button.ContainsMouse)
            {
                Environment.Cursor = MouseCursor.Hand;
            }

            Button.Update(position, width, height);
        }
    }
}
