using MonoGameDrawingApp.Ui.Base.Popup;
using MonoGameDrawingApp.Ui.Base.Tabs;
using MonoGameDrawingApp.Ui.FileSystemTrees;

namespace MonoGameDrawingApp.Ui.DrawingApp.Tabs.VectorSprites
{
    public class VectorSpriteOpenableFileType : TabOpenableFileType
    {
        public override string[] Extentions => new string[] { "vecspr" };

        public VectorSpriteOpenableFileType(PopupEnvironment popupEnvironment)
        {
            PopupEnvironment = popupEnvironment;
        }

        public PopupEnvironment PopupEnvironment { get; init; }

        protected override FileTab MakeTab(string path, TabEnvironment tabEnvironment)
        {
            return new VectorSpriteTab(tabEnvironment.Environment, path, PopupEnvironment);
        }
    }
}
