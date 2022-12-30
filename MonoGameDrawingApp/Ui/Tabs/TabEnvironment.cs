using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameDrawingApp.Ui.Scroll;
using MonoGameDrawingApp.Ui.Split.Vertical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameDrawingApp.Ui.Tabs
{
    public class TabEnvironment : IUiElement
    {
        public readonly TabBar TabBar;
        public readonly IUiElement Background;

        private readonly ScrollWindow _scrollWindow;

        private readonly VSplitStandard _vSplit;

        public TabEnvironment(IUiElement background, SpriteFont font)
        {
            TabBar = new TabBar(font);
            _scrollWindow = new ScrollWindow(TabBar, false, true);
            _scrollWindow.HScrollBar.ScrollSpeed = 0.25f;
            Background = background;
            _vSplit = new VSplitStandard(_scrollWindow, Background, 0);
        }

        public int RequiredWidth => _vSplit.RequiredWidth;

        public int RequiredHeight => _vSplit.RequiredHeight;

        public Texture2D Render(Graphics graphics, Vector2 position, int width, int height)
        {
            Tab selected = TabBar.SelectedTab;
            _vSplit.Second = selected == null ? Background : selected.Child;
            _vSplit.SplitPosition = TabBar.RequiredHeight + 10;
            return _vSplit.Render(graphics, position, width, height);
        }
    }
}
