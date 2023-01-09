﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameDrawingApp.Ui.Lists;
using System.Collections.Generic;

namespace MonoGameDrawingApp.Ui.Tabs
{
    public class TabBar : IUiElement
    {
        public SpriteFont Font;

        private List<TabView> _tabs;

        private HListView<TabView> _hListView;
        private Tab _selectedTab;

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

        public TabBar(SpriteFont font)
        {
            Font = font;
            _tabs = new List<TabView>();
            _hListView = new HListView<TabView>(_tabs);
        }

        public int RequiredWidth => _hListView.RequiredWidth;

        public int RequiredHeight => _hListView.RequiredHeight;

        public bool Changed => _hListView.Changed;

        public Texture2D Render(Graphics graphics, int width, int height)
        {
            return _hListView.Render(graphics, width, height);
        }

        public void OpenTab(Tab tab) 
        {
            _tabs.Add(new TabView(tab, Font));
            tab.TabBar = this;
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
