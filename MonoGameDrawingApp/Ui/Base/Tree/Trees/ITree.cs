using MonoGameDrawingApp.Ui.Base.Tree.TreeItems;

namespace MonoGameDrawingApp.Ui.Base.Tree.Trees
{
    public interface ITree
    {
        ITreeItem Root { get; }

        ITreeItem Selected { get; set; }

        void BackgroundRightClicked();
        void BackgroundLeftClicked();
    }
}
