using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameDrawingApp.Ui.Base.Themes;

namespace MonoGameDrawingApp.Ui.Base.Buttons
{
    public class TextButton : IUiElement
    {
        private readonly TextView _textView;
        private readonly ColorModifier _colorModifier;

        public TextButton(UiEnvironment environment, string text)
        {
            Text = text;
            Environment = environment;
            _textView = new TextView(environment, text);
            _colorModifier = new ColorModifier(environment, _textView, Theme.DefaultTextColor);
            Button = new Button(environment, _colorModifier);
        }


        public bool Changed => Button.Changed;

        public int RequiredWidth => Button.RequiredWidth;

        public int RequiredHeight => Button.RequiredHeight;

        public UiEnvironment Environment { get; }

        private ITheme Theme => Environment.Theme;

        public Button Button { get; }

        public string Text { get; }

        public Texture2D Render(Graphics graphics, int width, int height)
        {
            return Button.Render(graphics, width, height);
        }

        public void Update(Vector2 position, int width, int height)
        {
            _colorModifier.Color = Button.ContainsMouse ? Theme.HoveringTextColor : Theme.DefaultTextColor;

            if (Button.ContainsMouse)
            {
                Environment.Cursor = MouseCursor.Hand;
            }

            Button.Update(position, width, height);
        }
    }
}
