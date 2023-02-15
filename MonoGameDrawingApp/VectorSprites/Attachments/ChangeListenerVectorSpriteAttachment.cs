using System;

namespace MonoGameDrawingApp.VectorSprites.Attachments
{
    public class ChangeListenerVectorSpriteAttachment : IVectorSpriteItemAttachment
    {
        public Action Changed { get; init; }

        public ChangeListenerVectorSpriteAttachment(Action Changed)
        {
            this.Changed = Changed;
        }

        public void Attach(VectorSpriteItem item)
        {
            item.AddAttachment(this);
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
