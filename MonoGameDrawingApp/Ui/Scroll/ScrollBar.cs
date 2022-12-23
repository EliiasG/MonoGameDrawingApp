using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using MonoGameDrawingApp.Ui.Split;
using Microsoft.Xna.Framework.Input;

namespace MonoGameDrawingApp.Ui.Scroll
{
    public abstract class ScrollBar : IUiElement
    {
        private int _position = 0;
        private int oldScroll;

        protected BaseSplit _outer;
        protected BaseSplit _inner;

        public int RequiredWidth => 2;
        public int RequiredHeight => 2;

        public IUiElement FirstBackground;
        public IUiElement SecondBackground;
        public IUiElement Bar;

        public float ScrollSpeed = 10;
        public int End;
        public int Size;

        public ScrollBar()
        {
            FirstBackground = new ColorRect(Color.Transparent);
            SecondBackground = new ColorRect(Color.Transparent);
            Bar = new ColorRect(Color.LightGray);
        }

        public int Position
        {
            get
            {
                _position = Math.Clamp(_position, 0, End - Size);
                return _position;
            }
            set
            {
                _position = value;
            }
        }

        public Texture2D Render(Graphics graphics, Vector2 position, int width, int height)
        {
            int maxPos = End - Size;
            if (maxPos == 0)
            {
                _outer.SplitPosition = 0;
            }
            else
            {
                _outer.SplitPosition = Position * _outer.MaxPosition / (maxPos + Size);
            }
            _inner.SplitPosition = Size * _outer.MaxPosition / End;

            _updateScroll(position, width, height);

            return _outer.Render(graphics, position, width, height);
        }

        private void _updateScroll(Vector2 positon, int width, int height)
        {
            MouseState mouse = Mouse.GetState();
            int scroll = mouse.ScrollWheelValue;

            Rectangle collider = new Rectangle(positon.ToPoint(), new Point(width, height));
            if(collider.Contains(mouse.Position))
            {
                Position += (int) ((oldScroll - scroll) * ScrollSpeed);
            }

            oldScroll = scroll;
        }
    }
}
