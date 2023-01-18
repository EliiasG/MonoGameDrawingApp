using MonoGameDrawingApp.Ui.Tree.TreeItems;

namespace MonoGameDrawingApp.Ui.Tree.Trees
{
    public interface ITree
    {
        ITreeItem Root { get; }

        ITreeItem Selected { get; set; }

        bool HideRoot { get; }
    }
}
