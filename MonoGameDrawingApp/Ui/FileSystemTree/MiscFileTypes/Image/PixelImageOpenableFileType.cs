using MonoGameDrawingApp.Ui.Tabs;

namespace MonoGameDrawingApp.Ui.FileSystemTree.MiscFileTypes.Png
{
    public class PixelImageOpenableFileType : IOpenableFileType
    {
        public string[] Extentions => new string[] { "png", "jpg", "jpeg", "jfif", "pjpeg", "pjp" };

        public void Open(string path, TabEnvironment tabEnvironment)
        {
            tabEnvironment.TabBar.OpenTab(new ImageTab(tabEnvironment.Environment, path), true);
        }
    }
}
