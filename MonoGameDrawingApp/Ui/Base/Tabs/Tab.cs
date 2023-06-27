namespace MonoGameDrawingApp.Ui.Base.Tabs
{
    public abstract class Tab
    {
        public abstract IUiElement Child { get; }
        public abstract bool HasCloseButton { get; }
        public abstract string Title { get; }
        public bool IsSelected { get; set; }
        public TabBar TabBar { get; set; }

        public abstract void Close();

        public void ForceClose()
        {
            TabBar.CloseTab(this);
        }
    }
}
