using MonoGameDrawingApp.Ui.Tabs;

namespace MonoGameDrawingApp.Ui.FileSystemTree
{
    public interface IOpenableFileType
    {
        string[] Extentions { get; }

        void Open(string path, TabEnvironment tabEnvironment);
    }
}
