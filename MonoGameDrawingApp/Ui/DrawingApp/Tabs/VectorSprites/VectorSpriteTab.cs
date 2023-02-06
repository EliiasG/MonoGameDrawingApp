using MonoGameDrawingApp.Ui.Base;
using MonoGameDrawingApp.Ui.Base.Popup;
using MonoGameDrawingApp.Ui.FileSystemTrees;

namespace MonoGameDrawingApp.Ui.DrawingApp.Tabs.VectorSprites
{
    public class VectorSpriteTab : FileTab
    {
        private VectorSpriteTabView _spriteTabView;

        public VectorSpriteTab(UiEnvironment environment, string path, PopupEnvironment popupEnvironment) : base(path)
        {
            _spriteTabView = new VectorSpriteTabView(environment, path, popupEnvironment);
        }

        public override IUiElement Child => _spriteTabView;

        public override bool HasCloseButton => true;

        public override string Title => System.IO.Path.GetFileName(Path);

        protected override void _close()
        {
            ForceClose();
        }
    }
}
