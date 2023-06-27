using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MonoGameDrawingApp.Ui.Base
{
    public class MinSize : IUiElement
    {
        public MinSize(UiEnvironment environment, IUiElement child, int width, int height)
        {
            Environment = environment;
            Child = child;
            MinWidth = width;
            MinHeight = height;
        }

        public int RequiredWidth => Math.Max(MinWidth, Child.RequiredWidth);

        public int RequiredHeight => Math.Max(MinHeight, Child.RequiredHeight);

        public bool Changed => Child.Changed;

        public UiEnvironment Environment { get; }
        public int MinHeight { get; set; }
        public int MinWidth { get; set; }

        public IUiElement Child { get; }

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
