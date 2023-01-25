using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameDrawingApp.Ui.Split.Horizontal;
using MonoGameDrawingApp.Ui.Split.Vertical;
using System;

namespace MonoGameDrawingApp.Ui.Scroll
{
    public class ScrollWindow : IUiElement
    {
        public readonly bool IsLeft;
        public readonly bool IsTop;
        public readonly bool AllowHoverScrolling;

        public Vector2 Position = Vector2.Zero;

        public readonly VScrollBar VScrollBar;
        public readonly HScrollBar HScrollBar;

        public int ScrollBarSize = 10;

        public readonly IUiElement _corner;

        private readonly UiEnvironment _environment;

        private MouseState _oldMouse;

        private IScrollableView _child;

        private VSplit _outer;
        private HSplit _main;
        private HSplit _scrollBar;

        public ScrollWindow(UiEnvironment environment, IScrollableView child, bool isLeft = false, bool isTop = false, bool allowHoverScrolling = true)
        {
            _environment = environment;

            IsLeft = isLeft;
            IsTop = isTop;
            AllowHoverScrolling = allowHoverScrolling;
            _child = child;

            VScrollBar = new VScrollBar(environment);
            HScrollBar = new HScrollBar(environment);

            _child = child;
            _corner = new ColorRect(environment, Color.Transparent);

            if (isLeft)
            {
                _main = new HSplitStandard(environment, VScrollBar, _child, 1);
                _scrollBar = new HSplitStandard(environment, _corner, HScrollBar, 1);
            }
            else
            {
                _main = new HSplitStandard(environment, _child, VScrollBar, 1);
                _scrollBar = new HSplitStandard(environment, HScrollBar, _corner, 1);
            }
            if (isTop)
            {
                _outer = new VSplitStandard(environment, _scrollBar, _main, 1);
            }
            else
            {
                _outer = new VSplitStandard(environment, _main, _scrollBar, 1);
            }
        }

        public int RequiredWidth => 1 + HScrollBar.RequiredWidth;

        public int RequiredHeight => 1 + VScrollBar.RequiredHeight;

        public bool Changed => _outer.Changed;

        public UiEnvironment Environment => _environment;

        public void Update(Vector2 position, int width, int height)
        {
            _updateSplits(width, height);
            _updateBars(width, height);
            if (AllowHoverScrolling)
            {
                _updateHoverScrolling(position, width, height);
            }
            _updatePosition();
            _outer.Update(position, width, height);
        }

        public Texture2D Render(Graphics graphics, int width, int height)
        {
            return _outer.Render(graphics, width, height);
        }

        private void _updateSplits(int width, int height)
        {
            bool hVisible = _child.Height < _child.MaxHeight;
            int hDist =  hVisible ? ScrollBarSize : 1;
            int hPos = IsLeft ? hDist : width - hDist;
            _main.SplitPosition = hPos;
            _scrollBar.SplitPosition = hPos;

            bool vVisible = _child.Width < _child.MaxWidth;
            int vDist = vVisible ? ScrollBarSize : 1;
            int vPos = IsTop ? vDist : height - vDist;
            _outer.SplitPosition = vPos;

            HScrollBar.Disabled = !vVisible;
            VScrollBar.Disabled = !hVisible;
        }

        private void _updateBars(int width, int height) 
        {
            VScrollBar.Size = Math.Min(_child.MaxHeight, _child.Height);
            VScrollBar.End = _child.MaxHeight;

            HScrollBar.Size = Math.Min(_child.MaxWidth, _child.Width);
            HScrollBar.End = _child.MaxWidth;
        }

        private void _updatePosition()
        {
            _child.Position = new Vector2(HScrollBar.Position, VScrollBar.Position);
        }

        private void _updateHoverScrolling(Vector2 position, int width, int height)
        {
            MouseState mouse = Mouse.GetState();
            Point boundsPosition = position.ToPoint() + new Point(IsLeft ? ScrollBarSize : 0, IsTop ? ScrollBarSize : 0);
            Point boundsSize = new Point(width - ScrollBarSize, height - ScrollBarSize);
            if (!new Rectangle(boundsPosition, boundsSize).Contains(mouse.Position))
            {
                return;
            }
            bool shift = Keyboard.GetState().IsKeyDown(Keys.LeftShift);
            
            int scrollDiff = _oldMouse.ScrollWheelValue - mouse.ScrollWheelValue;

            if (shift || VScrollBar.Size >= VScrollBar.End)
            {
                HScrollBar.Position += (int)(scrollDiff * HScrollBar.ScrollSpeed);
            }
            else
            {
                VScrollBar.Position += (int)(scrollDiff * VScrollBar.ScrollSpeed);
            }

            _oldMouse = mouse;
        }
    }
}
