using MonoGameDrawingApp.Ui.Base.Tabs;

namespace MonoGameDrawingApp.Ui.FileSystemTrees
{
    public abstract class FileTab : Tab
    {
        public string Path;

        public FileTab(string path)
        {
            Path = path;
        }
    }
}
