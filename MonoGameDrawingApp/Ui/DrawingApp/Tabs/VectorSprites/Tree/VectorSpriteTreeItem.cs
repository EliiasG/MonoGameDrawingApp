using Microsoft.Xna.Framework.Input;
using MonoGameDrawingApp.Ui.Base.Popup;
using MonoGameDrawingApp.Ui.Base.Tree.TreeItems;
using MonoGameDrawingApp.Ui.Base.Tree.Trees;
using MonoGameDrawingApp.VectorSprites;
using System.Collections.Generic;
using System.Linq;

namespace MonoGameDrawingApp.Ui.DrawingApp.Tabs.VectorSprites.Tree
{
    public class VectorSpriteTreeItem : ITreeItem, IVectorSpriteItemAttachment
    {
        private readonly VectorSpriteTree _tree;

        private bool _isOpen = false;

        public VectorSpriteTreeItem(VectorSpriteTree tree, PopupEnvironment popupEnvironment)
        {
            _tree = tree;
            PopupEnvironment = popupEnvironment;
        }

        public PopupEnvironment PopupEnvironment { get; init; }

        public VectorSpriteItem Item { get; private set; }

        public string Name => Item.Name;

        public bool IsOpen
        {
            get
            {
                _isOpen = _isOpen && HasOpenButton;
                return _isOpen;
            }
            set
            {
                _isOpen = value;
            }
        }

        public bool HasOpenButton => Item.Children.Any();

        public string IconPath => "icons/spriteitem";

        public ITree Tree => _tree;

        public IEnumerable<ITreeItem> Children
        {
            get
            {
                return Item.Children.Select((VectorSpriteItem item) => item.GetAttachment<VectorSpriteTreeItem>());
            }
        }

        public void Attach(VectorSpriteItem item)
        {
            VectorSpriteTreeItem newItem = new(_tree, PopupEnvironment)
            {
                Item = item
            };
            item.AddAttachment(newItem);
        }

        public void Clicked()
        {
            _tree.Selected = this;
        }

        public void RightClicked()
        {
            //TODO open contextmenu
            VectorSpriteItemContextMenu contextMenu = new VectorSpriteItemContextMenu(PopupEnvironment.Environment, Item, PopupEnvironment);

            PopupEnvironment.Open(Mouse.GetState().Position, contextMenu);
        }
    }
}
