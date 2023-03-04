namespace MonoGameDrawingApp.VectorSprites.Attachments.UndoRedo
{
    public class UndoRedoVectorSpriteItemAttachment : IVectorSpriteItemAttachment
    {
        public UndoRedoVectorSpriteItemAttachment(VectorSpriteItem item, UndoRedoVectorSpiteAttachment spiteAttachment)
        {
            Item = item;
            SpiteAttachment = spiteAttachment;
        }

        public VectorSpriteItem Item { get; init; }

        public UndoRedoVectorSpiteAttachment SpiteAttachment { get; init; }

        public void ChildrenChanged()
        {
            SpiteAttachment._childrenChanged(Item);
        }

        public void DataChanged()
        {
            SpiteAttachment._dataChanged(Item);
        }

        public void ChildrenChanging()
        {
            SpiteAttachment._childrenChanging(Item);
        }

        public void DataChanging()
        {
            SpiteAttachment._dataChanging(Item);
        }
    }
}
