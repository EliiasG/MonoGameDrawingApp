using Microsoft.Xna.Framework.Input;
using MonoGameDrawingApp.Ui.Base.Popup;
using MonoGameDrawingApp.Ui.Base.Tree.TreeItems;
using MonoGameDrawingApp.Ui.Base.Tree.Trees;
using MonoGameDrawingApp.Ui.FileSystemTrees.Popup.ContextMenus;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MonoGameDrawingApp.Ui.FileSystemTrees.Items
{
    public class DirectoryTreeItem : IFileSystemTreeItem
    {
        private readonly PopupEnvironment _popupEnvironment;

        private readonly FileTypeManager _fileTypeManager;

        private readonly string _path;

        private List<IFileSystemTreeItem> _children;

        private readonly FileSystemTree _tree;
        private readonly ISet<string> _unauthorizedChildren;

        private bool _isOpen = false;

        public DirectoryTreeItem(string path, FileSystemTree tree, PopupEnvironment popupEnvironment, FileTypeManager fileTypeManager)
        {
            _path = path;
            _tree = tree;
            _children = new List<IFileSystemTreeItem>();
            _popupEnvironment = popupEnvironment;
            _unauthorizedChildren = new HashSet<string>();
            _fileTypeManager = fileTypeManager;
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
                    items = Directory.EnumerateDirectories(Path).Concat(Directory.EnumerateFiles(Path)).ToArray();
                }
                else
                {
                    items = DriveInfo.GetDrives().Select(x => x.Name).ToArray();
                }

                foreach (IFileSystemTreeItem child in _children)
                {
                    if (!Directory.Exists(child.Path) && !File.Exists(child.Path))
                    {
                        _children = new List<IFileSystemTreeItem>(_children);// setting _children to a clone, otherwise the loop will cause an error
                        _children.Remove(child);
                    }
                }

                int index = 0;

                foreach (string item in items)
                {
                    if (!_unauthorizedChildren.Contains(item) && !_children.Any((child) => child.Path == item)) //item is not unauthorized, and none of the paths are the item
                    {
                        if (Directory.Exists(item))
                        {
                            try
                            {
                                Directory.EnumerateFileSystemEntries(item); //Does nothing, just to throw exeption
                                _children.Insert(index, new DirectoryTreeItem(item, _tree, PopupEnvironment, FileTypeManager));
                            }
                            catch (UnauthorizedAccessException)
                            {
                                _unauthorizedChildren.Add(item);
                            }
                        }
                        else
                        {
                            //item must exist and is not a directory, so must be a file
                            _children.Insert(index, new FileTreeItem(item, Tree, PopupEnvironment, FileTypeManager));
                        }
                    }
                    if (!_unauthorizedChildren.Contains(item))
                    {
                        ++index;
                    }
                }

                return _children;
            }
        }

        public ITree Tree => _tree;

        public PopupEnvironment PopupEnvironment => _popupEnvironment;

        public FileTypeManager FileTypeManager => _fileTypeManager;

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
            PopupEnvironment.Open(Mouse.GetState().Position, new DirectoryContextMenu(PopupEnvironment.Environment, Path, PopupEnvironment, false, FileTypeManager));
        }
    }
}
