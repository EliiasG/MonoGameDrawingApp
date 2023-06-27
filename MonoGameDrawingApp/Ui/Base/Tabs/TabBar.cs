using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameDrawingApp.Ui.Base.Lists;
using MonoGameDrawingApp.Ui.Base.Scroll;
using System.Collections.Generic;

namespace MonoGameDrawingApp.Ui.Base.Tabs
{
    public class TabBar : IScrollableView
    {
        private readonly List<TabView> _tabs;

        private readonly HScrollableListView _hListView;
        private Tab _selectedTab;

        public TabBar(UiEnvironment environment)
        {
            Environment = environment;

            _tabs = new List<TabView>();
            _hListView = new HScrollableListView(environment, _tabs, false, 3);
        }


        public int RequiredWidth => 1;

        public int RequiredHeight => (int)Environment.FontHeight + 4;

        public bool Changed => _hListView.Changed;

        public UiEnvironment Environment { get; }

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
                    //maybe hacky but should not cause recursion, as this only runs when SelectedTab is set to null, and it sets SelectedTab to something that should not be null
                    SelectedTab = _tabs[0].Tab;
                }
            }
        }

        public Texture2D Render(Graphics graphics, int width, int height)
        {
            return _hListView.Render(graphics, width, height);
        }

        public void OpenTab(Tab tab, bool select = false)
        {
            _tabs.Add(new TabView(Environment, tab));
            tab.TabBar = this;
            if (select)
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

        public IEnumerable<T> GetTabsOfType<T>() where T : Tab
        {
            return _tabs.FindAll((TabView v) => v.Tab is T).ConvertAll((TabView v) => (T)v.Tab);
        }
    }
}
