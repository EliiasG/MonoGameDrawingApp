using System;

namespace MonoGameDrawingApp.VectorSprites.Attachments.ChangeListener
{
    public class ChangeListenerVectorSpriteAttachment : IVectorSpriteAttachment
    {
        private readonly ChangeListenerVectorSpriteItemAttachment _attachment;

        public ChangeListenerVectorSpriteAttachment(Action Changed)
        {
            this.Changed = Changed;
            _attachment = new ChangeListenerVectorSpriteItemAttachment(Changed);
        }

        public Action Changed { get; init; }

        public void Attach(VectorSpriteItem item)
        {
            item.AddAttachment(_attachment);
        }
    }
}
