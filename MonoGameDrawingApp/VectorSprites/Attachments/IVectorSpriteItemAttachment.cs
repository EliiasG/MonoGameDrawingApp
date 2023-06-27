namespace MonoGameDrawingApp.VectorSprites.Attachments
{
    public interface IVectorSpriteItemAttachment
    {
        void ChildrenChanged() { }

        void DataChanged() { }

        void ChildrenChanging() { }

        void DataChanging() { }
    }
}
