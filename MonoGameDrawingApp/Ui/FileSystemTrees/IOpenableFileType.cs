using MonoGameDrawingApp.Ui.Base.Tabs;

namespace MonoGameDrawingApp.Ui.FileSystemTrees
{
    public interface IOpenableFileType
    {
        string[] Extentions { get; }

        void Open(string path, TabEnvironment tabEnvironment);
    }
}
