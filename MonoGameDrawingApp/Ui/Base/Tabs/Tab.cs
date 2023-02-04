namespace MonoGameDrawingApp.Ui.Base.Tabs
{
    public abstract class Tab
    {
        public TabBar TabBar;
        public bool IsSelected = false;

        public abstract IUiElement Child { get; }
        public abstract bool HasCloseButton { get; }
        public abstract string Title { get; }

        protected abstract void _close();

        public void Close()
        {
            _close();
        }

        public void ForceClose()
        {
            TabBar.CloseTab(this);
        }
    }
}
