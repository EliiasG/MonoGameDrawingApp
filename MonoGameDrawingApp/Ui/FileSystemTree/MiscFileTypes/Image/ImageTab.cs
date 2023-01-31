using MonoGameDrawingApp.Ui.Base;

namespace MonoGameDrawingApp.Ui.FileSystemTree.MiscFileTypes.Png
{
    public class ImageTab : FileTab
    {
        private ImageTabView _pngTabView;

        public ImageTab(UiEnvironment environment, string path) : base(path)
        {
            _pngTabView = new ImageTabView(environment, path);
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
