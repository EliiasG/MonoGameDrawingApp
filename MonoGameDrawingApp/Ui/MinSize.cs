using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MonoGameDrawingApp.Ui
{
    public class MinSize : IUiElement
    {
        public int MinWidth;
        public int MinHeight;
        public IUiElement Child;
        public int RequiredWidth => Math.Max(MinWidth, Child.RequiredWidth);

        public int RequiredHeight => Math.Max(MinHeight, Child.RequiredHeight);

        public MinSize(IUiElement child, int width, int height)
        {
            Child = child;
            MinWidth = width;
            MinHeight = height;
        }

        public Texture2D Render(Graphics graphics, Vector2 position, int width, int height)
        {
            return Child.Render(graphics, position, width, height);
        }
    }
}
