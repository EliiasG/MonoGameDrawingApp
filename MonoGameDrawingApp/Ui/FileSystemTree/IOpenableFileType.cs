using MonoGameDrawingApp.Ui.Tabs;

namespace MonoGameDrawingApp.Ui.FileSystemTree
{
    public interface IOpenableFileType
    {
        string Extention { get; }

        void Open(string path, TabEnvironment tabEnvironment);
    }
}
