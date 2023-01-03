using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MonoGameDrawingApp.Ui
{
    public class MinSize : IUiElement
    {
        public readonly int MinWidth;
        public readonly int MinHeight;
        public readonly IUiElement Child;
        public int RequiredWidth => Math.Max(MinWidth, Child.RequiredWidth);

        public int RequiredHeight => Math.Max(MinHeight, Child.RequiredHeight);

        public bool Changed => Child.Changed;

        public MinSize(IUiElement child, int width, int height)
        {
            Child = child;
            MinWidth = width;
            MinHeight = height;
        }

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
