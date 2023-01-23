using MonoGameDrawingApp.Ui.Popup;
using MonoGameDrawingApp.Ui.Tree.TreeItems;
using MonoGameDrawingApp.Ui.Tree.TreeItems.FileSystem;
using System.Diagnostics;

namespace MonoGameDrawingApp.Ui.Tree.Trees
{
    public class FileSystemTree : ITree
    {
        public readonly bool CanSelectDirectories;

        public readonly bool CanSelectFiles;

        private readonly ITreeItem _root;

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
                else if (value is null && CanSelectFiles) //TODO replace null with FileTreeItem
                {
                    _selected = value;
                }
            }
        }

        public void BackgroundLeftClicked()
        {
            Debug.WriteLine("Left");//TODO
        }

        public void BackgroundRightClicked()
        {
            Debug.WriteLine("Right");//TODO
        }
    }
}
