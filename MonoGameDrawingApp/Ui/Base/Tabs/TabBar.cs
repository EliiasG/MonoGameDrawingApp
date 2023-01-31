using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameDrawingApp.Ui.Base;
using MonoGameDrawingApp.Ui.List;
using MonoGameDrawingApp.Ui.Scroll;
using System.Collections.Generic;

namespace MonoGameDrawingApp.Ui.Tabs
{
    public class TabBar : IScrollableView
    {
        private readonly UiEnvironment _environment;

        private List<TabView> _tabs;

        private HScrollableListView _hListView;
        private Tab _selectedTab;

        public TabBar(UiEnvironment environment)
        {
            _environment = environment;

            _tabs = new List<TabView>();
            _hListView = new HScrollableListView(environment, _tabs, false, 3);
        }


        public int RequiredWidth => 1;

        public int RequiredHeight => (int)_Environment.FontHeight + 4;

        public bool Changed => _hListView.Changed;

        public UiEnvironment Environment => _environment;

        public Vector2 Position
        {
            get => _hListView.Position;
            set => _hListView.Position = value;
        }

        public int Width => _hListView.Width;

        public int Height => _hListView.Height;

        public int MaxWidth => _hListView.MaxWidth;

        public int MaxHeight => _hListView.MaxHeight;

        public Tab SelectedTab
        {
            get => _selectedTab;
            set
            {
                if (_selectedTab != null)
                {
                    _selectedTab.IsSelected = false;
                }
                _selectedTab = value;
                if (_selectedTab != null)
                {
                    _selectedTab.IsSelected = true;
                }
                else if (_tabs.Count > 0)
                {
                    SelectedTab = _tabs[0].Tab;
                }
            }
        }

        public Texture2D Render(Graphics graphics, int width, int height)
        {
            return _hListView.Render(graphics, width, height);
        }

        public void OpenTab(Tab tab, bool open = false)
        {
            _tabs.Add(new TabView(Environment, tab));
            tab.TabBar = this;
            if (open)
            {
                SelectedTab = tab;
            }
        }

        public void CloseTab(Tab tab)
        {
            foreach (TabView tabView in _tabs)
            {
                if (tabView.Tab == tab)
                {
                    tab.TabBar = null;
                    _tabs.Remove(tabView);
                    if (tab.IsSelected)
                    {
                        SelectedTab = null;
                    }
                    return;
                }
            }
        }

        public void Update(Vector2 position, int width, int height)
        {
            _hListView.Update(position, width, height);
        }
    }
}
