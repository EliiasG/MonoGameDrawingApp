using MonoGameDrawingApp.Ui.Tabs;

namespace MonoGameDrawingApp.Ui.FileSystemTree
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
