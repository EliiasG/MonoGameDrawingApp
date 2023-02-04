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
        private readonly PopupEnvironment _popupEnvironment;

        private readonly FileTypeManager _fileTypeManager;

        private readonly string _path;

        private readonly ITree _tree;

        public FileTreeItem(string path, ITree tree, PopupEnvironment popupEnvironment, FileTypeManager fileOpener)
        {
            _path = path;
            _tree = tree;
            _popupEnvironment = popupEnvironment;
            _fileTypeManager = fileOpener;
        }

        public string Path => _path;

        public string Name => System.IO.Path.GetFileName(Path);

        public bool IsOpen { get => false; set { } }

        public bool HasOpenButton => false;

        public string IconPath => _fileTypeManager.GetIconPath(Path);

        public IEnumerable<ITreeItem> Children => null;

        public ITree Tree => _tree;

        public FileTypeManager FileTypeManager => _fileTypeManager;

        public PopupEnvironment PopupEnvironment => _popupEnvironment;

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
