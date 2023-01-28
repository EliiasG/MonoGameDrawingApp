using Microsoft.Xna.Framework.Input;
using MonoGameDrawingApp.Ui.Base.Popup.ContextMenu.Menus.FileSystem;
using MonoGameDrawingApp.Ui.Base.Tree.TreeItems;
using MonoGameDrawingApp.Ui.FileSystemTree.FileSystem;
using MonoGameDrawingApp.Ui.Popup;

namespace MonoGameDrawingApp.Ui.Tree.Trees
{
    public class FileSystemTree : ITree
    {
        public readonly bool CanSelectDirectories;

        public readonly bool CanSelectFiles;

        private readonly DirectoryTreeItem _root;

        private ITreeItem _selected = null;

        public FileSystemTree(string path, PopupEnvironment popupEnvironment, bool canSelectDirectories = false, bool canSelectFiles = false)
        {
            _root = new DirectoryTreeItem(path, this, popupEnvironment);
            _root.IsOpen = true;
            CanSelectFiles = canSelectFiles;
            CanSelectDirectories = canSelectDirectories;
        }

        public ITreeItem Root => _root;

        public ITreeItem Selected
        {
            get => _selected;
            set
            {
                if (value == null)
                {
                    _selected = null;
                }
                else if (value is DirectoryTreeItem && CanSelectDirectories)
                {
                    _selected = value;
                }
                else if (value is FileTreeItem && CanSelectFiles)
                {
                    _selected = value;
                }
            }
        }

        public void BackgroundLeftClicked()
        {
            Selected = null;
        }

        public void BackgroundRightClicked()
        {
            DirectoryContextMenu contextMenu = new DirectoryContextMenu(_root.PopupEnvironment.Environment, _root.Path, _root.PopupEnvironment, true);
            _root.PopupEnvironment.Open(Mouse.GetState().Position, contextMenu);
        }
    }
}
