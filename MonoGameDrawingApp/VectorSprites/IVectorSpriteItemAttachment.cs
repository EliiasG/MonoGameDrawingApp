namespace MonoGameDrawingApp.VectorSprites
{
    public interface IVectorSpriteItemAttachment
    {
        void Attach(VectorSpriteItem item);

        void ChildrenChanged() { }

        void DataChanged() { }
    }
}
