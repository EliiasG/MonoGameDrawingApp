using System;

namespace MonoGameDrawingApp.VectorSprites.Attachments.ChangeListener
{
    public class ChangeListenerVectorSpriteAttachment : IVectorSpriteAttachment
    {
        private readonly ChangeListenerVectorSpriteItemAttachment _attachment;

        public ChangeListenerVectorSpriteAttachment(Action Changed)
        {
            this.Changed = Changed;
            _attachment = new ChangeListenerVectorSpriteItemAttachment(this);
        }

        public Action Changed { get; set; }

        public void Attach(VectorSpriteItem item)
        {
            item.AddAttachment(_attachment);
        }
    }
}
