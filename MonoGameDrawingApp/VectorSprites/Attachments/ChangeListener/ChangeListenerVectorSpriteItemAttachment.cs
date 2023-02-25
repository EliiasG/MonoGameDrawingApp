namespace MonoGameDrawingApp.VectorSprites.Attachments.ChangeListener
{
    public class ChangeListenerVectorSpriteItemAttachment : IVectorSpriteItemAttachment
    {

        public ChangeListenerVectorSpriteItemAttachment(ChangeListenerVectorSpriteAttachment vectorSpriteAttachment)
        {
            VectorSpriteAttachment = vectorSpriteAttachment;
        }

        public ChangeListenerVectorSpriteAttachment VectorSpriteAttachment { get; init; }

        public void ChildrenChanged()
        {
            VectorSpriteAttachment.Changed();
        }

        public void DataChanged()
        {
            VectorSpriteAttachment.Changed();
        }
    }
}
