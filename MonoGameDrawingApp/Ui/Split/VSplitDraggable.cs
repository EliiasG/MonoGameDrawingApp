using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;

namespace MonoGameDrawingApp.Ui.Split
{
    public class VSplitDraggable : VSplit
    {

        public readonly int HandleHeight;
        private readonly VSplit _outer;
        private readonly VSplit _bottom;
        private int _dragOffset = -1;

        public IUiElement Splitter;
        public bool _wasPressed = false;


        public VSplitDraggable(IUiElement first, IUiElement second, int splitPosition, int handleHeight) : base(first, second, splitPosition)
        {
            HandleHeight = handleHeight;
            Splitter = new ColorRect(Color.White);
            /* layout:
            outer:
                First
                bottom:
                    Splitter
                    Second
            */
            _bottom = new VSplitStandard(Splitter, Second, handleHeight);

            _outer = new VSplitStandard(First, _bottom, splitPosition);
        }

        protected override Texture2D _render(Graphics graphics, Vector2 position)
        {
            Texture2D outerRender = _outer.Render(graphics, position, _width, _height);
            ElementBuilder elementBuilder = new ElementBuilder(graphics, _width, _height);
            graphics.SpriteBatch.Draw(
                texture: outerRender,
                position: new Vector2(0),
                color: Color.White
            );
            _updateSplitPosition(position);
            _outer.SplitPosition = SplitPosition;
            _bottom.SplitPosition = HandleHeight;
            return elementBuilder.Finish();
        }

        public override int MaxPosition => base.MaxPosition - HandleHeight;

        public override int RequiredHeight => base.RequiredHeight + HandleHeight;

        private void _updateSplitPosition(Vector2 position)
        {
            MouseState mouse = Mouse.GetState();
            position = position + new Vector2(0, _outer.SplitPosition);

            bool left = mouse.LeftButton == ButtonState.Pressed;

            bool justPressed = left && !_wasPressed;
            _wasPressed = left;

            if (!left)
            {
                _dragOffset = -1;
                return;
            }

            
            bool isInVertical = mouse.Y >= position.Y && mouse.Y <= position.Y + HandleHeight;
            bool isInHorizontal = mouse.X >= position.X && mouse.X <= position.X + _width;
            
            if(justPressed && isInVertical && isInHorizontal)
            {
                _dragOffset = (int) position.Y - mouse.Y;
            }

            if(left && _dragOffset != -1)
            {
                SplitPosition = mouse.Y + _dragOffset;
            }
        }
    }
}
