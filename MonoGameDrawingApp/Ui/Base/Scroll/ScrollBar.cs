using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameDrawingApp.Ui.Base.Split;
using System;

namespace MonoGameDrawingApp.Ui.Base.Scroll
{
    public abstract class ScrollBar : IUiElement
    {
        public int RequiredWidth => 2;
        public int RequiredHeight => 2;

        private int _position;
        private int _dragOffset = -1;
        private MouseState _oldMouse;

        public ScrollBar(UiEnvironment environment)
        {
            Environment = environment;

            FirstBackground = new ColorRect(environment, Color.Transparent);
            SecondBackground = new ColorRect(environment, Color.Transparent);
            Bar = new ColorRect(environment, environment.Theme.ScrollbarColor);
            BarHovering = new ColorRect(environment, environment.Theme.HoveringButtonColor);
            BarDisabled = new ColorRect(environment, Color.Transparent);
        }

        protected abstract Rectangle GetBarBounds(Vector2 position, int width, int height, int dist, int length);
        protected abstract int GetMouseOffset(Vector2 position, int width, int height);
        public int Position
        {
            get
            {
                _position = Math.Clamp(_position, 0, End - Size);
                return _position;
            }
            set => _position = value;
        }

        public bool Changed => Outer.Changed;

        public UiEnvironment Environment { get; }
        public bool Disabled { get; set; }

        protected IUiElement BarHovering { get; }

        protected IUiElement BarDisabled { get; }

        protected IUiElement FirstBackground { get; }

        protected IUiElement SecondBackground { get; }

        protected IUiElement Bar { get; }

        protected BaseSplit Inner { get; set; }
        protected ChangeableView InnerBar { get; set; }
        protected BaseSplit Outer { get; set; }
        public int Size { get; set; }
        public int End { get; set; }
        public float ScrollSpeed { get; set; } = 0.5f;

        public void Update(Vector2 position, int width, int height)
        {
            Outer.Update(position, width, height);
            int maxPos = End - Size;
            int dist = Size == 0 ? 0 : Position * Outer.MaxPosition / (maxPos + Size);
            if (maxPos == 0)
            {
                Outer.SplitPosition = 0;
            }
            else
            {
                Outer.SplitPosition = dist;
            }
            int length = End == 0 ? 0 : Size * Outer.MaxPosition / End;
            Inner.SplitPosition = length;

            UpdateScroll(position, width, height);
            UpdateBarDrag(position, width, height, dist, length);

            _oldMouse = Mouse.GetState();
        }

        public Texture2D Render(Graphics graphics, int width, int height)
        {
            return Outer.Render(graphics, width, height);
        }

        private void UpdateScroll(Vector2 positon, int width, int height)
        {
            MouseState mouse = Mouse.GetState();
            int scroll = mouse.ScrollWheelValue;

            Rectangle collider = new(positon.ToPoint(), new Point(width, height));
            if (collider.Contains(mouse.Position))
            {
                Position += (int)((_oldMouse.ScrollWheelValue - scroll) * ScrollSpeed);
            }
        }

        private void UpdateBarDrag(Vector2 positon, int width, int height, int dist, int length)
        {
            MouseState mouse = Mouse.GetState();

            int offset = GetMouseOffset(positon, width, height);

            bool hovering = GetBarBounds(positon, width, height, dist, length).Contains(mouse.Position);
            bool clicked = mouse.LeftButton == ButtonState.Pressed;
            bool justClicked = clicked && !(_oldMouse.LeftButton == ButtonState.Pressed);

            if (Disabled)
            {
                InnerBar.Child = BarDisabled;
                return;
            }

            if (hovering || _dragOffset != -1)
            {
                InnerBar.Child = BarHovering;
                if (justClicked)
                {
                    _dragOffset = offset - Position;
                }
            }
            else
            {
                InnerBar.Child = Bar;
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
