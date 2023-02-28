using MonoGameDrawingApp.Ui.Base;

namespace MonoGameDrawingApp.Ui.FileSystemTrees.MiscFileTypes.Image
{
    public class ImageTab : FileTab
    {
        private readonly ImageTabView _imageTabView;

        public ImageTab(UiEnvironment environment, string path) : base(path)
        {
            _imageTabView = new ImageTabView(environment, path);
        }

        public override IUiElement Child => _imageTabView;

        public override bool HasCloseButton => true;

        public override string Title => System.IO.Path.GetFileName(Path);

        protected override void _close()
        {
            ForceClose();
        }
    }
}
