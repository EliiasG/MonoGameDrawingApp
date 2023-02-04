using MonoGameDrawingApp.Ui.Base.Tabs;

namespace MonoGameDrawingApp.Ui.FileSystemTrees.MiscFileTypes.Image
{
    public class PixelImageOpenableFileType : TabOpenableFileType
    {
        public override string[] Extentions => new string[] { "png", "jpg", "jpeg", "jfif", "pjpeg", "pjp" };

        protected override FileTab _makeTab(string path, TabEnvironment tabEnvironment)
        {
            return new ImageTab(tabEnvironment.Environment, path);
        }
    }
}
