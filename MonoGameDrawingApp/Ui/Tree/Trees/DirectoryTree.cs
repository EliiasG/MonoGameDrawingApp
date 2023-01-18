using MonoGameDrawingApp.Ui.Tree.TreeItems;
using MonoGameDrawingApp.Ui.Tree.TreeItems.FileSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameDrawingApp.Ui.Tree.Trees
{
    public class DirectoryTree : ITree
    {
        public readonly bool CanSelectDirectories;

        public readonly bool CanSelectFiles;

        private readonly ITreeItem _root;

        private ITreeItem _selected = null;

        public DirectoryTree(string path, bool canSelectDirectories = false, bool canSelectFiles = false)
        {
            _root = new DirectoryTreeItem(path, this);
            CanSelectFiles = canSelectFiles;
            CanSelectDirectories = canSelectDirectories;
        }

        public ITreeItem Root => _root;

        public bool HideRoot => false;

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
    }
}
