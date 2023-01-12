using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using MonoGameDrawingApp.Ui.Split;
using Microsoft.Xna.Framework.Input;

namespace MonoGameDrawingApp.Ui.Scroll
{
    public abstract class ScrollBar : IUiElement
    { 
        public int RequiredWidth => 2;
        public int RequiredHeight => 2;

        public IUiElement FirstBackground;
        public IUiElement SecondBackground;
        public IUiElement Bar;
        public IUiElement BarHovering;

        public float ScrollSpeed = 10;
        public int End;
        public int Size;

        private readonly UiEnvironment _environment;

        private int _position = 0;
        private int _dragOffset = -1;
        private MouseState _oldMouse;

        protected BaseSplit _outer;
        protected BaseSplit _inner;
        protected ChangeableView _innerBar;

        public ScrollBar(UiEnvironment environment)
        {
            _environment = environment;

            FirstBackground = new ColorRect(environment, Color.Transparent);
            SecondBackground = new ColorRect(environment, Color.Transparent);
            Bar = new ColorRect(environment, Color.Gray);
            BarHovering = new ColorRect(environment, Color.LightGray);
        }

        protected abstract Rectangle _getBarBounds(Vector2 position, int width, int height, int dist, int length);
        protected abstract int _getMouseOffset(Vector2 position, int width, int height);
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

        public bool Changed => _outer.Changed;

        public UiEnvironment Environment => _environment;

        public void Update(Vector2 position, int width, int height)
        {
            _outer.Update(position, width, height);
            int maxPos = End - Size;
            int dist = Position * _outer.MaxPosition / (maxPos + Size);
            if (maxPos == 0)
            {
                _outer.SplitPosition = 0;
            }
            else
            {
                _outer.SplitPosition = dist;
            }
            int length = Size * _outer.MaxPosition / End;
            _inner.SplitPosition = length;

            _updateScroll(position, width, height);
            _updateBarDrag(position, width, height, dist, length);

            _oldMouse = Mouse.GetState();
        }

        public Texture2D Render(Graphics graphics, int width, int height)
        {
            return _outer.Render(graphics, width, height);
        }

        private void _updateScroll(Vector2 positon, int width, int height)
        {
            MouseState mouse = Mouse.GetState();
            int scroll = mouse.ScrollWheelValue;

            Rectangle collider = new Rectangle(positon.ToPoint(), new Point(width, height));
            if(collider.Contains(mouse.Position))
            {
                Position += (int) ((_oldMouse.ScrollWheelValue - scroll) * ScrollSpeed);
            }
        }

        private void _updateBarDrag(Vector2 positon, int width, int height, int dist, int length)
        {
            MouseState mouse = Mouse.GetState();

            int offset = _getMouseOffset(positon, width, height);

            bool hovering = _getBarBounds(positon, width, height, dist, length).Contains(mouse.Position);
            bool clicked = mouse.LeftButton == ButtonState.Pressed;
            bool justClicked = clicked && !(_oldMouse.LeftButton == ButtonState.Pressed);

            if (hovering || _dragOffset != -1)
            {
                _innerBar.Child = BarHovering;
                if(justClicked)
                {
                    _dragOffset = offset - Position;
                }
            }
            else
            {
                _innerBar.Child = Bar;
            }

            if (_dragOffset != -1) 
            {
                Position = offset - _dragOffset;
            }

            if (!clicked)
            {
                _dragOffset = -1;
            }
        }
    }
}
