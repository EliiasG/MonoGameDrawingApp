using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoGameDrawingApp.Ui.Base.Split.Horizontal
{
    public class HSplitDraggable : HSplit
    {
        private readonly HSplit _outer;
        private readonly HSplit _left;
        private int _dragOffset = -1;

        public HSplitDraggable(UiEnvironment environment, IUiElement first, IUiElement second, int splitPosition, int handleWidth) : base(environment, first, second, splitPosition)
        {
            HandleWidth = handleWidth;
            Splitter = new ColorRect(environment, Color.Transparent);
            /* layout:
            outer:
                First
                bottom:
                    Splitter
                    Second
            */
            _left = new HSplitStandard(environment, Splitter, Second, handleWidth);

            _outer = new HSplitStandard(environment, First, _left, splitPosition);
        }

        protected override Texture2D Render(Graphics graphics)
        {
            return _outer.Render(graphics, Width, Height);
        }

        public override int MaxPosition => base.MaxPosition - HandleWidth;

        public override int RequiredWidth => base.RequiredWidth + HandleWidth;

        public bool WasPressed { get; set; }
        public IUiElement Splitter { get; set; }

        public int HandleWidth { get; }

        public override void Update(Vector2 position, int width, int height)
        {
            _outer.SplitPosition = SplitPosition;
            _left.SplitPosition = HandleWidth;

            _outer.Update(position, width, height);

            if (_outer.Changed)
            {
                Changed = true;
            }

            MouseState mouse = Mouse.GetState();
            Vector2 barPosition = position + new Vector2(_outer.SplitPosition, 0);

            bool left = mouse.LeftButton == ButtonState.Pressed;

            bool justPressed = left && !WasPressed;
            WasPressed = left;

            bool isInVertical = mouse.Y >= barPosition.Y && mouse.Y <= barPosition.Y + Height;
            bool isInHorizontal = mouse.X >= barPosition.X && mouse.X <= barPosition.X + HandleWidth;
            bool isIn = isInVertical && isInHorizontal;

            if (isIn || _dragOffset != -1)
            {
                Environment.Cursor = MouseCursor.SizeWE;
            }

            if (!left)
            {
                _dragOffset = -1;
                return;
            }

            if (justPressed && isIn)
            {
                _dragOffset = (int)barPosition.X - mouse.X;
            }

            if (left && _dragOffset != -1)
            {
                SplitPosition = mouse.X - (int)position.X + _dragOffset;
                Environment.LockCursor();
            }
        }
    }
}
