using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MonoGameDrawingApp.Ui.Base.Buttons
{
    public class SmartButton : IUiElement
    {
        public SmartButton(UiEnvironment environment, IUiElement child, Action leftClicked = null, Action rightClicked = null)
        {
            Environment = environment;

            Button = new Button(Environment, child);

            LeftClicked = leftClicked;
            RightClicked = rightClicked;
        }

        public Button Button { get; init; }

        public Action LeftClicked { get; set; }

        public Action RightClicked { get; set; }

        public bool Changed => Button.Changed;

        public int RequiredWidth => Button.RequiredWidth;

        public int RequiredHeight => Button.RequiredHeight;

        public UiEnvironment Environment { get; init; }

        public Texture2D Render(Graphics graphics, int width, int height)
        {
            return Button.Render(graphics, width, height);
        }

        public void Update(Vector2 position, int width, int height)
        {
            if (Button.JustLeftClicked)
            {
                LeftClicked?.Invoke();
            }

            if (Button.JustRightClicked)
            {
                RightClicked?.Invoke();
            }

            Button.Update(position, width, height);
        }
    }
}
