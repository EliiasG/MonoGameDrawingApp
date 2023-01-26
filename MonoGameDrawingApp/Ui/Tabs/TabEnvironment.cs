﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameDrawingApp.Ui.Scroll;
using MonoGameDrawingApp.Ui.Split.Vertical;

namespace MonoGameDrawingApp.Ui.Tabs
{
    public class TabEnvironment : IUiElement
    {
        public readonly TabBar TabBar;
        public readonly IUiElement Background;

        private readonly UiEnvironment _environment;

        private readonly ScrollWindow _scrollWindow;
        private readonly VSplit _outer;
        private readonly VSplit _inner;
        private readonly ChangeableView _changeableView;

        public TabEnvironment(UiEnvironment environment, IUiElement background)
        {
            _environment = environment;

            TabBar = new TabBar(environment);
            _scrollWindow = new ScrollWindow(environment, TabBar, false, true);
            _scrollWindow.HScrollBar.ScrollSpeed = 0.5f;
            Background = background;
            _changeableView = new ChangeableView(environment, Background);
            _inner = new VSplitStandard(environment, _scrollWindow, new ColorRect(environment, environment.Theme.ButtonColor), 0);
            _outer = new VSplitStandard(environment, _inner, _changeableView, 0);
        }

        public int RequiredWidth => _outer.RequiredWidth;

        public int RequiredHeight => _outer.RequiredHeight;

        public bool Changed => _outer.Changed;

        public UiEnvironment Environment => _environment;

        public Texture2D Render(Graphics graphics, int width, int height)
        {
            Tab selected = TabBar.SelectedTab;
            _changeableView.Child = selected?.Child ?? Background;
            _outer.SplitPosition = TabBar.RequiredHeight + 15;
            _inner.SplitPosition = TabBar.RequiredHeight + 10;


            return _outer.Render(graphics, width, height);
        }

        public void Update(Vector2 position, int width, int height)
        {
            _outer.Update(position, width, height);
        }
    }
}
