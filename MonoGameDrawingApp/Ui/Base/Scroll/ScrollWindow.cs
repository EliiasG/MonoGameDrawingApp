using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameDrawingApp.Ui.Base.Split.Horizontal;
using MonoGameDrawingApp.Ui.Base.Split.Vertical;
using System;

namespace MonoGameDrawingApp.Ui.Base.Scroll
{
    public class ScrollWindow : IUiElement
    {
        private MouseState _oldMouse;

        private readonly IScrollableView _child;

        private readonly VSplit _outer;
        private readonly HSplit _main;
        private readonly HSplit _scrollBar;

        public ScrollWindow(UiEnvironment environment, IScrollableView child, bool isLeft = false, bool isTop = false, bool allowHoverScrolling = true)
        {
            Environment = environment;

            IsLeft = isLeft;
            IsTop = isTop;
            AllowHoverScrolling = allowHoverScrolling;
            _child = child;

            VScrollBar = new VScrollBar(environment);
            HScrollBar = new HScrollBar(environment);

            _child = child;
            Corner = new ColorRect(environment, Color.Transparent);

            if (isLeft)
            {
                _main = new HSplitStandard(environment, VScrollBar, _child, 1);
                _scrollBar = new HSplitStandard(environment, Corner, HScrollBar, 1);
            }
            else
            {
                _main = new HSplitStandard(environment, _child, VScrollBar, 1);
                _scrollBar = new HSplitStandard(environment, HScrollBar, Corner, 1);
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

        public UiEnvironment Environment { get; }

        public bool IsLeft { get; }

        public bool IsTop { get; }

        public bool AllowHoverScrolling { get; }
        public Vector2 Position { get; set; }

        public HScrollBar HScrollBar { get; }

        public VScrollBar VScrollBar { get; }
        public int ScrollBarSize { get; set; } = 10;

        public IUiElement Corner { get; }

        public void Update(Vector2 position, int width, int height)
        {
            UpdateSplits(width, height);
            UpdateBars();
            if (AllowHoverScrolling)
            {
                UpdateHoverScrolling(position, width, height);
            }
            UpdatePosition();
            _outer.Update(position, width, height);
        }

        public Texture2D Render(Graphics graphics, int width, int height)
        {
            return _outer.Render(graphics, width, height);
        }

        private void UpdateSplits(int width, int height)
        {
            bool hVisible = _child.Height < _child.MaxHeight;
            int hDist = hVisible ? ScrollBarSize : 1;
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

        private void UpdateBars()
        {
            VScrollBar.Size = Math.Min(_child.MaxHeight, _child.Height);
            VScrollBar.End = _child.MaxHeight;

            HScrollBar.Size = Math.Min(_child.MaxWidth, _child.Width);
            HScrollBar.End = _child.MaxWidth;
        }

        private void UpdatePosition()
        {
            _child.Position = new Vector2(HScrollBar.Position, VScrollBar.Position);
        }

        private void UpdateHoverScrolling(Vector2 position, int width, int height)
        {
            MouseState mouse = Mouse.GetState();
            Point boundsPosition = position.ToPoint() + new Point(IsLeft ? ScrollBarSize : 0, IsTop ? ScrollBarSize : 0);
            Point boundsSize = new(width - ScrollBarSize, height - ScrollBarSize);
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
