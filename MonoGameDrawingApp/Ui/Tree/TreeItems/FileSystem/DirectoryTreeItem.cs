using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MonoGameDrawingApp.Ui.Popup;
using MonoGameDrawingApp.Ui.Tree.Trees;

namespace MonoGameDrawingApp.Ui.Tree.TreeItems.FileSystem
{
    public class DirectoryTreeItem : IFileSystemTreeItem
    {
        public readonly PopupEnvironment PopupEnvironment;

        private readonly string _path;

        private readonly List<IFileSystemTreeItem> _children;
        private readonly ITree _tree;

        private bool _isOpen;

        public DirectoryTreeItem(string path, ITree tree, PopupEnvironment popupEnvironment)
        {
            _path = path;
            _tree = tree;
            _children = new List<IFileSystemTreeItem>();
            PopupEnvironment = popupEnvironment;
        }

        public string Path => _path;

        public string Name => Path.Split("\\").Last().Length == 0 ? Path : System.IO.Path.GetFileNameWithoutExtension(Path);

        public bool IsOpen
        {
            get
            {
                _isOpen = _isOpen && HasOpenButton;
                return _isOpen;
            }
            set => _isOpen = value;
        }

        public bool HasOpenButton
        {
            get 
            {
                return true;
                /*
                try
                {
                    return Path == "" || Directory.EnumerateFileSystemEntries(Path).Any();
                }
                catch
                {
                    return true;
                }
                */
            }   
        }

        public string IconPath => "icons/folder";

        public IEnumerable<ITreeItem> Children
        {
            get
            {
                if(!IsOpen)
                {
                    return null;
                }

                string[] items;
                if (Path != "")
                {

                    items = Directory.GetFileSystemEntries(Path);
                }
                else
                {
                    items = DriveInfo.GetDrives().Select(x => x.Name).ToArray();
                }
                

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
                            _children.Add(new DirectoryTreeItem(item, Tree, PopupEnvironment));
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
            Tree.Selected = this;
        }

        public void RightClicked()
        {
            throw new NotImplementedException();
            //TODO
        }
    }
}
