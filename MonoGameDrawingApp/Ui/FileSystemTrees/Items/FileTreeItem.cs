using Microsoft.Xna.Framework.Input;
using MonoGameDrawingApp.Ui.Base.Popup;
using MonoGameDrawingApp.Ui.Base.Tree.TreeItems;
using MonoGameDrawingApp.Ui.Base.Tree.Trees;
using MonoGameDrawingApp.Ui.FileSystemTrees.Popup.ContextMenus;
using System.Collections.Generic;

namespace MonoGameDrawingApp.Ui.FileSystemTrees.Items
{
    public class FileTreeItem : IFileSystemTreeItem
    {
        public FileTreeItem(string path, ITree tree, PopupEnvironment popupEnvironment, FileTypeManager fileOpener)
        {
            Path = path;
            Tree = tree;
            PopupEnvironment = popupEnvironment;
            FileTypeManager = fileOpener;
        }

        public string Path { get; }

        public string Name => System.IO.Path.GetFileName(Path);

        public bool IsOpen { get => false; set { } }

        public bool HasOpenButton => false;

        public string IconPath => FileTypeManager.GetIconPath(Path);

        public IEnumerable<ITreeItem> Children => null;

        public ITree Tree { get; }

        public FileTypeManager FileTypeManager { get; }

        public PopupEnvironment PopupEnvironment { get; }

        public void Clicked()
        {
            if (Tree.Selected == this)
            {
                FileTypeManager.OpenFile(Path);
            }

            Tree.Selected = this;
        }

        public void RightClicked()
        {
            PopupEnvironment.Open(Mouse.GetState().Position, new FileContextMenu(PopupEnvironment.Environment, Path, PopupEnvironment, FileTypeManager));
        }
    }
}
