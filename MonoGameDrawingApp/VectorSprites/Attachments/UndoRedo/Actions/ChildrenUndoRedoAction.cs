using MonoGameDrawingApp.VectorSprites.Attachments.UndoRedo.Snapshots;

namespace MonoGameDrawingApp.VectorSprites.Attachments.UndoRedo.Actions
{
    public class ChildrenUndoRedoAction : IUndoRedoAction
    {
        private readonly VectorSpriteItemChildrenSnapshot _before;
        private VectorSpriteItemChildrenSnapshot _after;


        public ChildrenUndoRedoAction(VectorSpriteItem item)
        {
            Item = item;
            _before = new VectorSpriteItemChildrenSnapshot(item);
        }

        public VectorSpriteItem Item { get; init; }

        public void Done()
        {
            _after = new VectorSpriteItemChildrenSnapshot(Item);
        }

        public void Redo()
        {
            _after.Apply(Item);
        }

        public void Undo()
        {
            _before.Apply(Item);
        }
    }
}
