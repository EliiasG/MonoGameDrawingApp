using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoGameDrawingApp.Ui.Base.Split.Horizontal
{
    public class HSplitDraggable : HSplit
    {
        //Edited copy-paste of VSplitDraggable, maybe i should refactor it at some point
        //Problably never gonna happen
        public readonly int HandleWidth;
        private readonly HSplit _outer;
        private readonly HSplit _left;
        private int _dragOffset = -1;

        public IUiElement Splitter;
        public bool _wasPressed = false;

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

        protected override Texture2D _render(Graphics graphics)
        {
            return _outer.Render(graphics, _width, _height);
        }

        public override int MaxPosition => base.MaxPosition - HandleWidth;

        public override int RequiredWidth => base.RequiredWidth + HandleWidth;

        public override void Update(Vector2 position, int width, int height)
        {
            _outer.SplitPosition = SplitPosition;
            _left.SplitPosition = HandleWidth;

            _outer.Update(position, width, height);

            if (_outer.Changed)
            {
                _changed = true;
            }

            MouseState mouse = Mouse.GetState();
            Vector2 barPosition = position + new Vector2(_outer.SplitPosition, 0);

            bool left = mouse.LeftButton == ButtonState.Pressed;

            bool justPressed = left && !_wasPressed;
            _wasPressed = left;

            bool isInVertical = mouse.Y >= barPosition.Y && mouse.Y <= barPosition.Y + _height;
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
