using MonoGameDrawingApp.Ui.Base;

namespace MonoGameDrawingApp.Ui.FileSystemTree.MiscFileTypes.Png
{
    public class PngTab : FileTab
    {
        private PngTabView _pngTabView;

        public PngTab(UiEnvironment environment, string path) : base(path)
        {
            _pngTabView = new PngTabView(environment, path);
        }

        public override IUiElement Child => _pngTabView;

        public override bool HasCloseButton => true;

        public override string Title => System.IO.Path.GetFileName(Path);

        protected override void _closeButtonPressed()
        {
            Close();
        }
    }
}
