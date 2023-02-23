using System;

namespace MonoGameDrawingApp.VectorSprites.Attachments.ChangeListener
{
    public class ChangeListenerVectorSpriteItemAttachment : IVectorSpriteAttachment
    {
        public Action Changed { get; init; }

        public ChangeListenerVectorSpriteItemAttachment(Action Changed)
        {
            this.Changed = Changed;
        }

        public void ChildrenChanged()
        {
            Changed();
        }

        public void DataChanged()
        {
            Changed();
        }
    }
}
