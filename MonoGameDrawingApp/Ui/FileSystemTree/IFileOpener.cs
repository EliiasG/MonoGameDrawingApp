using MonoGameDrawingApp.Ui.Tabs;

namespace MonoGameDrawingApp.Ui.FileSystemTree
{
    public interface IFileOpener
    {
        void Open(string path, TabEnvironment tabEnvironment);
    }
}
