namespace MonoGameDrawingApp.VectorSprites.Attachments
{
    public interface IVectorSpriteItemAttachment
    {
        void Attach(VectorSpriteItem item);

        void ChildrenChanged() { }

        void DataChanged() { }
    }
}
