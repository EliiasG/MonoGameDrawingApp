using System;

namespace MonoGameDrawingApp.VectorSprites.Attachments.ChangeListener
{
    public class ChangeListenerVectorSpriteItemAttachment : IVectorSpriteItemAttachment
    {

        public ChangeListenerVectorSpriteItemAttachment(ChangeListenerVectorSpriteAttachment vectorSpriteAttachment)
        {
            VectorSpriteAttachment = vectorSpriteAttachment;
        }

        public ChangeListenerVectorSpriteAttachment VectorSpriteAttachment { get; init; }

        public Action Changed { get; set; }

        public void ChildrenChanged()
        {
            VectorSpriteAttachment.Changed();
            Changed?.Invoke();
        }

        public void DataChanged()
        {
            VectorSpriteAttachment.Changed();
            Changed?.Invoke();
        }


    }
}
