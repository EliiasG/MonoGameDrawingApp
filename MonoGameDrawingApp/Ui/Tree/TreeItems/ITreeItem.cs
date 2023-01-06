using System.Collections.Generic;

namespace MonoGameDrawingApp.Ui.Tree.TreeItems
{
    public interface ITreeItem
    {
        public string Name { get; }

        public bool IsOpen { get; set; }

        public bool HasOpenButton { get; }

        public IUiElement Icon { get; }

        public void Clicked();

        public void RightClicked();

        public IEnumerable<ITreeItem> Children { get; }
    }
}
