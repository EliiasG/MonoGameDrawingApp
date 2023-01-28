using MonoGameDrawingApp.Ui.Base.Tree.TreeItems;
using MonoGameDrawingApp.Ui.Popup;
using MonoGameDrawingApp.Ui.Tree.Trees;
using System.Collections.Generic;

namespace MonoGameDrawingApp.Ui.FileSystemTree.FileSystem
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

        public string Name => System.IO.Path.GetFileName(Path);

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
