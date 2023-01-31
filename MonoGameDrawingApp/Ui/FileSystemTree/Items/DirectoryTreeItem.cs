using Microsoft.Xna.Framework.Input;
using MonoGameDrawingApp.Ui.Base.Tree.TreeItems;
using MonoGameDrawingApp.Ui.FileSystemTree.Popup.ContextMenus;
using MonoGameDrawingApp.Ui.Popup;
using MonoGameDrawingApp.Ui.Tree.Trees;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MonoGameDrawingApp.Ui.FileSystemTree.FileSystem
{
    public class DirectoryTreeItem : IFileSystemTreeItem
    {
        private readonly PopupEnvironment _popupEnvironment;

        private readonly FileTypeManager _fileOpener;

        private readonly string _path;

        private List<IFileSystemTreeItem> _children;

        private readonly ITree _tree;
        private readonly ISet<string> _unauthorizedChildren;

        private bool _isOpen = false;

        public DirectoryTreeItem(string path, ITree tree, PopupEnvironment popupEnvironment, FileTypeManager fileOpener)
        {
            _path = path;
            _tree = tree;
            _children = new List<IFileSystemTreeItem>();
            _popupEnvironment = popupEnvironment;
            _unauthorizedChildren = new HashSet<string>();
            _fileOpener = fileOpener;
        }

        public string Path => _path;

        public string Name => Path.Split("\\").Last().Length == 0 ? Path : System.IO.Path.GetFileName(Path);

        public bool IsOpen
        {
            get
            {
                _isOpen = _isOpen && HasOpenButton;
                return _isOpen;
            }
            set => _isOpen = value;
        }

        public bool HasOpenButton => true;

        public string IconPath => "icons/folder";

        public IEnumerable<ITreeItem> Children
        {
            get
            {

                if (!IsOpen)
                {
                    return null;
                }

                string[] items;
                if (Path != "")
                {
                    items = Directory.GetDirectories(Path).Concat(Directory.GetFiles(Path)).ToArray();
                }
                else
                {
                    items = DriveInfo.GetDrives().Select(x => x.Name).ToArray();
                }

                foreach (IFileSystemTreeItem child in _children)
                {
                    if (!items.Contains(child.Path))
                    {
                        _children = new List<IFileSystemTreeItem>(_children);// setting _children to a clone, otherwise the loop will cause an error
                        _children.Remove(child);
                    }
                }

                foreach (string item in items)
                {
                    if (!_unauthorizedChildren.Contains(item) && !_children.Any((child) => child.Path == item)) //item is not unauthorized, and none of the paths are the item
                    {
                        if (Directory.Exists(item))
                        {
                            try
                            {
                                Directory.EnumerateFileSystemEntries(item); //Does nothing, just to throw exeption
                                _children.Add(new DirectoryTreeItem(item, _tree, PopupEnvironment, FileTypeManager));
                                //  return _children;//for load-in effect
                            }
                            catch (UnauthorizedAccessException)
                            {
                                _unauthorizedChildren.Add(item);
                            }
                        }
                        else
                        {
                            //item must exist and is not a directory, so must be a file
                            _children.Add(new FileTreeItem(item, Tree, PopupEnvironment, FileTypeManager));
                            //return _children;
                        }
                    }
                }

                return _children;
            }
        }

        public ITree Tree => _tree;

        public PopupEnvironment PopupEnvironment => _popupEnvironment;

        public FileTypeManager FileTypeManager => _fileOpener;

        public void Clicked()
        {
            if (Tree.Selected == this)
            {
                _isOpen = true;
            }
            Tree.Selected = this;
        }

        public void RightClicked()
        {
            PopupEnvironment.Open(Mouse.GetState().Position, new DirectoryContextMenu(PopupEnvironment.Environment, Path, PopupEnvironment, false));
        }
    }
}
