using MonoGameDrawingApp.Ui.Base.Tree.TreeItems;
using MonoGameDrawingApp.Ui.Popup;

namespace MonoGameDrawingApp.Ui.FileSystemTree.FileSystem
{
    public interface IFileSystemTreeItem : ITreeItem
    {
        string Path { get; }

        PopupEnvironment PopupEnvironment { get; }

        FileTypeManager FileTypeManager { get; }
    }
}
