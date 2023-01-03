﻿namespace MonoGameDrawingApp.Ui.Tabs
{
    public abstract class Tab
    {
        public abstract IUiElement Child { get; }
        public abstract bool HasCloseButton { get; }
        public abstract string Title { get; }
        public TabBar TabBar;
        public bool IsSelected = false;

        protected abstract void _closeButtonPressed();

        public void Close()
        {
            TabBar.CloseTab(this);
        }
    }
}
