using MonoGameDrawingApp.Ui.Base.Popup;
using MonoGameDrawingApp.Ui.Base.Tree.TreeItems;
using MonoGameDrawingApp.Ui.Base.Tree.Trees;
using MonoGameDrawingApp.Ui.FileSystemTrees.Items;

namespace MonoGameDrawingApp.Ui.FileSystemTrees
{
    public class FileSystemTree : ITree
    {
        public readonly bool CanSelectDirectories;

        public readonly bool CanSelectFiles;

        private readonly DirectoryTreeItem _root;

        private ITreeItem _selected = null;

        public FileSystemTree(string path, PopupEnvironment popupEnvironment, FileTypeManager fileTypeManager, bool canSelectDirectories = false, bool canSelectFiles = false)
        {
            _root = new DirectoryTreeItem(path, this, popupEnvironment, fileTypeManager);
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
            _root.RightClicked();
        }
    }
}
