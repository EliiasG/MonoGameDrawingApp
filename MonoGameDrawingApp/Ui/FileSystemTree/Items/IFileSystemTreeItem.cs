using MonoGameDrawingApp.Ui.Base.Tree.TreeItems;

namespace MonoGameDrawingApp.Ui.FileSystemTree.FileSystem
{
    public interface IFileSystemTreeItem : ITreeItem
    {
        string Path { get; }
    }
}
