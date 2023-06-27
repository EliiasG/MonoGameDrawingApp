using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoGameDrawingApp.Ui.Base.Split.Vertical
{
    public class VSplitDraggable : VSplit
    {
        private readonly VSplit _outer;
        private readonly VSplit _bottom;
        private int _dragOffset = -1;

        public VSplitDraggable(UiEnvironment environment, IUiElement first, IUiElement second, int splitPosition, int handleHeight) : base(environment, first, second, splitPosition)
        {
            HandleHeight = handleHeight;
            Splitter = new ColorRect(environment, Color.Transparent);
            /* layout:
            outer:
                First
                bottom:
                    Splitter
                    Second
            */
            _bottom = new VSplitStandard(environment, Splitter, Second, handleHeight);

            _outer = new VSplitStandard(environment, First, _bottom, splitPosition);
        }

        protected override Texture2D Render(Graphics graphics)
        {
            return _outer.Render(graphics, Width, Height);
        }

        public override int MaxPosition => base.MaxPosition - HandleHeight;

        public override int RequiredHeight => base.RequiredHeight + HandleHeight;

        public int HandleHeight { get; }
        public IUiElement Splitter { get; set; }
        public bool WasPressed { get; set; }

        public override void Update(Vector2 position, int width, int height)
        {
            _outer.SplitPosition = SplitPosition;
            _bottom.SplitPosition = HandleHeight;

            _outer.Update(position, width, height);

            if (_outer.Changed)
            {
                Changed = true;
            }

            MouseState mouse = Mouse.GetState();
            Vector2 barPosition = position + new Vector2(0, _outer.SplitPosition);

            bool left = mouse.LeftButton == ButtonState.Pressed;

            bool justPressed = left && !WasPressed;
            WasPressed = left;

            bool isInVertical = mouse.Y >= barPosition.Y && mouse.Y <= barPosition.Y + HandleHeight;
            bool isInHorizontal = mouse.X >= barPosition.X && mouse.X <= barPosition.X + width;
            bool isIn = isInVertical && isInHorizontal;

            if (isIn || _dragOffset != -1)
            {
                Environment.Cursor = MouseCursor.SizeNS;
            }

            if (!left)
            {
                _dragOffset = -1;
                return;
            }

            if (justPressed && isIn)
            {
                _dragOffset = (int)barPosition.Y - mouse.Y;
            }

            if (left && _dragOffset != -1)
            {
                SplitPosition = mouse.Y - (int)position.Y + _dragOffset;
                Environment.LockCursor();
            }
        }
    }
}
