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

        public readonly IUiElement Child;

        public Vector2 Position = Vector2.Zero;

        public VScrollBar VScrollBar = new VScrollBar();
        public HScrollBar HScrollBar = new HScrollBar();

        public int ScrollBarSize = 10;

        public IUiElement Corner = new ColorRect(Color.Transparent);

        private MouseState _oldMouse;

        private PeekView _peekWindow;

        private VSplit _outer;
        private HSplit _main;
        private HSplit _scrollBar;

        public ScrollWindow(IUiElement child, bool isLeft = false, bool isTop = false, bool allowHoverScrolling = true)
        {
            IsLeft = isLeft;
            IsTop = isTop;
            AllowHoverScrolling = allowHoverScrolling;
            Child = child;

            VScrollBar = new VScrollBar();
            HScrollBar = new HScrollBar();

            _peekWindow = new PeekView(child);

            if (isLeft)
            {
                _main = new HSplitStandard(VScrollBar, _peekWindow, 1);
                _scrollBar = new HSplitStandard(Corner, HScrollBar, 1);
            }
            else
            {
                _main = new HSplitStandard(_peekWindow, VScrollBar, 1);
                _scrollBar = new HSplitStandard(HScrollBar, Corner, 1);
            }
            if (isTop)
            {
                _outer = new VSplitStandard(_scrollBar, _main, 1);
            }
            else
            {
                _outer = new VSplitStandard(_main, _scrollBar, 1);
            }
        }

        public int RequiredWidth => 1 + HScrollBar.RequiredWidth;

        public int RequiredHeight => 1 + VScrollBar.RequiredHeight;

        public bool Changed => _outer.Changed;

        public void Update(Vector2 position, int width, int height)
        {
            _updateSplits(width, height);
            _updateBars(width, height);
            if (AllowHoverScrolling)
            {
                _updateHoverScrolling(position, width, height);
            }
            _updatePosition();
        }

        public Texture2D Render(Graphics graphics, int width, int height)
        {
            return _outer.Render(graphics, width, height);
        }

        private void _updateSplits(int width, int height)
        {
            int hDist = width - ScrollBarSize < Child.RequiredHeight ? ScrollBarSize : 1;
            int hPos = IsLeft ? hDist : width - hDist;
            _main.SplitPosition = hPos;
            _scrollBar.SplitPosition = hPos;

            int vDist = height - ScrollBarSize < Child.RequiredWidth ? ScrollBarSize : 1;
            int vPos = IsTop ? vDist : height - vDist;
            _outer.SplitPosition = vPos;
        }

        private void _updateBars(int width, int height) 
        {
            VScrollBar.Size = Math.Min(Child.RequiredHeight, height - ScrollBarSize);
            VScrollBar.End = Child.RequiredHeight;

            HScrollBar.Size = Math.Min(Child.RequiredWidth, width - ScrollBarSize);
            HScrollBar.End = Child.RequiredWidth;
        }

        private void _updatePosition()
        {
            _peekWindow.Position = new Vector2(HScrollBar.Position, VScrollBar.Position);
        }

        private void _updateHoverScrolling(Vector2 position, int width, int height)
        {
            MouseState mouse = Mouse.GetState();
            if(!new Rectangle(position.ToPoint(), new Point(width - ScrollBarSize, height - ScrollBarSize)).Contains(mouse.Position))
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
