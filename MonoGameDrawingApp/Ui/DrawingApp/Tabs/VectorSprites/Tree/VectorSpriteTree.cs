using MonoGameDrawingApp.Ui.Base.Popup;
using MonoGameDrawingApp.Ui.Base.Tree.TreeItems;
using MonoGameDrawingApp.Ui.Base.Tree.Trees;
using MonoGameDrawingApp.VectorSprites;

namespace MonoGameDrawingApp.Ui.DrawingApp.Tabs.VectorSprites.Tree
{
    public class VectorSpriteTree : ITree
    {
        public VectorSpriteTree(VectorSprite sprite, PopupEnvironment popupEnvironment)
        {
            Sprite = sprite;
            sprite.AddAttachment(new VectorSpriteTreeAttachment(this, popupEnvironment));
        }

        public VectorSprite Sprite { get; init; }

        public ITreeItem Root => Sprite.Root.GetAttachment<VectorSpriteTreeItem>();

        public ITreeItem Selected { get; set; }

        public void BackgroundLeftClicked()
        {
            Selected = null;
        }

        public void BackgroundRightClicked()
        {
        }
    }
}