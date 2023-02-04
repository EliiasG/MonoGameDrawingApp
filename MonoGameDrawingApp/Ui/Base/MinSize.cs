using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MonoGameDrawingApp.Ui.Base
{
    public class MinSize : IUiElement
    {
        public int MinWidth;
        public int MinHeight;
        public readonly IUiElement Child;

        private readonly UiEnvironment _environment;

        public MinSize(UiEnvironment environment, IUiElement child, int width, int height)
        {
            _environment = environment;
            Child = child;
            MinWidth = width;
            MinHeight = height;
        }

        public int RequiredWidth => Math.Max(MinWidth, Child.RequiredWidth);

        public int RequiredHeight => Math.Max(MinHeight, Child.RequiredHeight);

        public bool Changed => Child.Changed;

        public UiEnvironment Environment => _environment;

        public Texture2D Render(Graphics graphics, int width, int height)
        {
            return Child.Render(graphics, width, height);
        }

        public void Update(Vector2 position, int width, int height)
        {
            Child.Update(position, width, height);
        }
    }
}
