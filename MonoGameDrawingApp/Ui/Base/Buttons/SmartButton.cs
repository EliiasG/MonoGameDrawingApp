using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MonoGameDrawingApp.Ui.Base.Buttons
{
    public class SmartButton : IUiElement
    {
        private Button _button;

        public SmartButton(UiEnvironment environment, IUiElement child, Action leftClicked = null, Action rightClicked = null)
        {
            Environment = environment;

            _button = new Button(Environment, child);

            LeftClicked = leftClicked;
            RightClicked = rightClicked;
        }

        public Action LeftClicked { get; set; }

        public Action RightClicked { get; set; }

        public bool Changed => _button.Changed;

        public int RequiredWidth => _button.RequiredWidth;

        public int RequiredHeight => _button.RequiredHeight;

        public UiEnvironment Environment { get; init; }

        public Texture2D Render(Graphics graphics, int width, int height)
        {
            return _button.Render(graphics, width, height);
        }

        public void Update(Vector2 position, int width, int height)
        {
            if (_button.JustLeftClicked)
            {
                LeftClicked?.Invoke();
            }

            if (_button.JustRightClicked)
            {
                RightClicked?.Invoke();
            }

            _button.Update(position, width, height);
        }
    }
}
