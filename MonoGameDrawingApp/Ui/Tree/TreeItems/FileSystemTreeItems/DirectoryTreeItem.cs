using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MonoGameDrawingApp.Ui.Tree.TreeItems.FileSystemTreeItems
{
    public class DirectoryTreeItem : IFileSystemTreeItem
    {
        private readonly string _path;

        private readonly List<IFileSystemTreeItem> _children;
        private readonly IUiElement _icon;
        private readonly ITree _tree;

        private bool _isOpen;

        public DirectoryTreeItem(string path, ITree tree)
        {
            _path = path;
            _icon = new SpriteView("icons/folder");
            _tree = tree;
            _children = new List<IFileSystemTreeItem>();
        }

        public string Path => _path;

        public string Name => System.IO.Path.GetFileNameWithoutExtension(Path);

        public bool IsOpen 
        {
            get
            {
                _isOpen = _isOpen && HasOpenButton;
                return _isOpen;
            }
            set => _isOpen = value; 
        }

        public bool HasOpenButton => !Directory.EnumerateFileSystemEntries(Path).Any();

        public IUiElement Icon => _icon;

        public IEnumerable<ITreeItem> Children
        {
            get
            {
                if(!IsOpen)
                {
                    return null;
                }

                string[] items = Directory.GetFileSystemEntries(Path);

                foreach (IFileSystemTreeItem child in _children) 
                {
                    if (!items.Contains(child.Path))
                    {
                        _children.Remove(child);
                    }
                }

                foreach (string item in items)
                {
                    if (_children.All((IFileSystemTreeItem child) => child.Path != item)) //All paths are not item == no paths are item
                    {
                        if (Directory.Exists(item))
                        {
                            _children.Add(new DirectoryTreeItem(item, Tree));
                        }
                        else
                        {
                            //item must exist and is not a directory, so must be a file
                            //TODO fix
                        }
                    }
                }

                return _children;
            }
        }

        public ITree Tree => _tree;

        public void Clicked()
        {
            throw new NotImplementedException();
            //TODO
        }

        public void RightClicked()
        {
            throw new NotImplementedException();
            //TODO
        }
    }
}
