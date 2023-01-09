using MonoGameDrawingApp.Ui.Tree.TreeItems;

namespace MonoGameDrawingApp.Ui.Tree
{
    public interface ITree
    {
        ITreeItem Root { get; }

        ITreeItem Selected { get; }

        bool HideRoot { get; }
    }
}
