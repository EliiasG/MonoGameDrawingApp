using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameDrawingApp.Ui.Scroll;
using MonoGameDrawingApp.Ui.Split.Vertical;

namespace MonoGameDrawingApp.Ui.Tabs
{
    public class TabEnvironment : IUiElement
    {
        public readonly TabBar TabBar;
        public readonly IUiElement Background;

        private readonly ScrollWindow _scrollWindow;
        private readonly VSplitStandard _vSplit;
        private readonly ChangeableView _changeableView;

        public TabEnvironment(IUiElement background, SpriteFont font)
        {
            TabBar = new TabBar(font);
            _scrollWindow = new ScrollWindow(TabBar, false, true);
            _scrollWindow.HScrollBar.ScrollSpeed = 0.25f;
            Background = background;
            _changeableView = new ChangeableView(Background);
            _vSplit = new VSplitStandard(_scrollWindow, _changeableView, 0);
        }

        public int RequiredWidth => _vSplit.RequiredWidth;

        public int RequiredHeight => _vSplit.RequiredHeight;

        public bool Changed => _vSplit.Changed;

        public Texture2D Render(Graphics graphics, int width, int height)
        {
            Tab selected = TabBar.SelectedTab;
            _changeableView.Child = selected == null ? Background : selected.Child;
            _vSplit.SplitPosition = TabBar.RequiredHeight + 10;

            return _vSplit.Render(graphics, width, height);
        }

        public void Update(Vector2 position, int width, int height)
        {
            _vSplit.Update(position, width, height);
        }
    }
}
