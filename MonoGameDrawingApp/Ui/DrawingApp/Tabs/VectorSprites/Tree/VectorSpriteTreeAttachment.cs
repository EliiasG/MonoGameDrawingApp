using MonoGameDrawingApp.Ui.Base.Popup;
using MonoGameDrawingApp.VectorSprites;
using MonoGameDrawingApp.VectorSprites.Attachments;

namespace MonoGameDrawingApp.Ui.DrawingApp.Tabs.VectorSprites.Tree
{
    public class VectorSpriteTreeAttachment : IVectorSpriteAttachment
    {
        private readonly VectorSpriteTree _tree;

        public VectorSpriteTreeAttachment(VectorSpriteTree tree, PopupEnvironment popupEnvironment)
        {
            _tree = tree;
            PopupEnvironment = popupEnvironment;
        }

        public PopupEnvironment PopupEnvironment { get; init; }

        public void Attach(VectorSpriteItem item)
        {
            VectorSpriteTreeItem newItem = new(_tree, PopupEnvironment, item);

            item.AddAttachment(newItem);
        }
    }
}
