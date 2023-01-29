using MonoGameDrawingApp.Ui.Tabs;

namespace MonoGameDrawingApp.Ui.FileSystemTree.MiscFileTypes.Png
{
    internal class PngOpenableFileType : IOpenableFileType
    {
        public string Extention => "png";

        public void Open(string path, TabEnvironment tabEnvironment)
        {
            tabEnvironment.TabBar.OpenTab(new PngTab(tabEnvironment.Environment, path), true);
        }
    }
}
