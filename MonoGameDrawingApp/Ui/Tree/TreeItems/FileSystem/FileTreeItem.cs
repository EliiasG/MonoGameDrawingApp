using Microsoft.Xna.Framework.Input;
using MonoGameDrawingApp.Ui.Popup.ContextMenu.Menus.FileSystem;
using MonoGameDrawingApp.Ui.Popup;
using MonoGameDrawingApp.Ui.Tree.Trees;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameDrawingApp.Ui.Tree.TreeItems.FileSystem
{
    public class FileTreeItem : IFileSystemTreeItem
    {
        public readonly PopupEnvironment PopupEnvironment;

        private readonly string _path;

        private readonly ITree _tree;

        public FileTreeItem(string path, ITree tree, PopupEnvironment popupEnvironment)
        {
            _path = path;
            _tree = tree;
            PopupEnvironment = popupEnvironment;
        }

        public string Path => _path;

        public string Name =>System.IO.Path.GetFileName(Path);

        public bool IsOpen { get => false; set { } }

        public bool HasOpenButton => false;

        public string IconPath => "icons/file";

        public IEnumerable<ITreeItem> Children => null;

        public ITree Tree => _tree;

        public void Clicked()
        {
            Tree.Selected = this;
        }

        public void RightClicked()
        {
            //TODO
            //PopupEnvironment.Open(Mouse.GetState().Position, new FileContextMenu(PopupEnvironment.Environment, Path, PopupEnvironment, false));
        }
    }
}
