using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameDrawingApp.Ui.Split.Vertical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameDrawingApp.Ui.Split.Horizontal
{
    public class HSplitDraggable : HSplit
    {
        //Edited copy-paste of VSplitDraggable, maybe i should refactor it at some point
        public readonly int HandleWidth;
        private readonly HSplit _outer;
        private readonly HSplit _left;
        private int _dragOffset = -1;
        private RenderHelper _renderHelper;

        public IUiElement Splitter;
        public bool _wasPressed = false;

        public HSplitDraggable(IUiElement first, IUiElement second, int splitPosition, int handleWidth) : base(first, second, splitPosition)
        {
            HandleWidth = handleWidth;
            Splitter = new ColorRect(Color.Transparent);
            /* layout:
            outer:
                First
                bottom:
                    Splitter
                    Second
            */
            _left = new HSplitStandard(Splitter, Second, handleWidth);

            _outer = new HSplitStandard(First, _left, splitPosition);

            _renderHelper = new RenderHelper();
        }

        protected override Texture2D _render(Graphics graphics, Vector2 position)
        {
            Texture2D outerRender = _outer.Render(graphics, position, _width, _height);
            _renderHelper.Begin(graphics, _width, _height);

            graphics.SpriteBatch.Draw(
                texture: outerRender,
                position: new Vector2(0),
                color: Color.White
            );
            _updateSplitPosition(position, graphics);
            _outer.SplitPosition = SplitPosition;
            _left.SplitPosition = HandleWidth;

            return _renderHelper.Finish();
        }

        public override int MaxPosition => base.MaxPosition - HandleWidth;

        public override int RequiredWidth => base.RequiredWidth + HandleWidth;

        private void _updateSplitPosition(Vector2 position, Graphics graphics)
        {
            MouseState mouse = Mouse.GetState();
            position = position + new Vector2(_outer.SplitPosition, 0);

            bool left = mouse.LeftButton == ButtonState.Pressed;

            bool justPressed = left && !_wasPressed;
            _wasPressed = left;

            bool isInVertical = mouse.Y >= position.Y && mouse.Y <= position.Y + _height;
            bool isInHorizontal = mouse.X >= position.X && mouse.X <= position.X + HandleWidth;
            bool isIn = isInVertical && isInHorizontal;

            if (isIn || _dragOffset != -1)
            {
                graphics.Cursor = MouseCursor.SizeWE;
            }

            if (!left)
            {
                _dragOffset = -1;
                return;
            }

            if (justPressed && isIn)
            {
                _dragOffset = (int)position.X - mouse.X;
            }

            if (left && _dragOffset != -1)
            {
                SplitPosition = mouse.X + _dragOffset;
            }
        }
    }
}
