using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameDrawingApp.Ui.Split.Horizontal;
using MonoGameDrawingApp.Ui.Split.Vertical;
using System;

namespace MonoGameDrawingApp.Ui.Scroll
{
    public class ScrollWindow : IUiElement
    {
        public readonly bool IsLeft;
        public readonly bool IsTop;

        public readonly IUiElement Child;

        public Vector2 Position = Vector2.Zero;

        public VScrollBar VScrollBar = new VScrollBar();
        public HScrollBar HScrollBar = new HScrollBar();

        public int ScrollBarSize = 10;

        public IUiElement Corner = new ColorRect(Color.Transparent);

        private PeekWindow _peekWindow;

        private VSplit _outer;
        private HSplit _main;
        private HSplit _scrollBar;

        public ScrollWindow(IUiElement child, bool isLeft = false, bool isTop = false)
        {
            IsLeft = isLeft;
            IsTop = isTop;
            Child = child;

            VScrollBar = new VScrollBar();
            HScrollBar = new HScrollBar();

            _peekWindow = new PeekWindow(child);
            //TODO implent scrolling when hovering over window
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

        public int RequiredWidth => 1;

        public int RequiredHeight => 1;

        public Texture2D Render(Graphics graphics, Vector2 position, int width, int height)
        {
            _updateSplits(width, height);
            _updateBars(width, height);
            _updatePosition();
            return _outer.Render(graphics, position, width, height);
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
            VScrollBar.Size = Math.Min(Child.RequiredHeight, height);
            VScrollBar.End = Child.RequiredHeight;

            HScrollBar.Size = Math.Min(Child.RequiredWidth, width);
            HScrollBar.End = Child.RequiredWidth;
        }

        private void _updatePosition()
        {
            _peekWindow.Position = new Vector2(HScrollBar.Position, VScrollBar.Position);
        }
    }
}
