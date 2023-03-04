using MonoGameDrawingApp.VectorSprites.Attachments.UndoRedo.Snapshots;

namespace MonoGameDrawingApp.VectorSprites.Attachments.UndoRedo.Actions
{
    public class DataUndoRedoAction : IUndoRedoAction
    {
        private readonly VectorSpriteItemDataSnapshot _before;

        private VectorSpriteItemDataSnapshot _after;

        public DataUndoRedoAction(VectorSpriteItem item)
        {
            Item = item;
            _before = new VectorSpriteItemDataSnapshot(Item);
        }

        public VectorSpriteItem Item { get; init; }

        public void Done()
        {
            _after = new VectorSpriteItemDataSnapshot(Item);
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
