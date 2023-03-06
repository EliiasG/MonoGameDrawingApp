using System;

namespace MonoGameDrawingApp.VectorSprites.Attachments.ChangeListener
{
    public class ChangeListenerVectorSpriteAttachment : IVectorSpriteAttachment
    {

        public ChangeListenerVectorSpriteAttachment(Action Changed)
        {
            this.Changed = Changed;
        }

        public Action Changed { get; set; }

        public void Attach(VectorSpriteItem item)
        {
            item.AddAttachment(new ChangeListenerVectorSpriteItemAttachment(this));
        }
    }
}
