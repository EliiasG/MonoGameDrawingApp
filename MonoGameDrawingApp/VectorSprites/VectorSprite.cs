using System.Collections.Generic;
using MonoGameDrawingApp.VectorSprites.Attachments;

namespace MonoGameDrawingApp.VectorSprites
{
    public class VectorSprite
    {
        private readonly List<IVectorSpriteItemAttachment> _attachmentList;

        public VectorSprite()
        {
            _attachmentList = new List<IVectorSpriteItemAttachment>();
        }

        public VectorSpriteItem Root { get; set; }

        public void AddAttachment(IVectorSpriteItemAttachment attachment)
        {
            _attachmentList.Add(attachment);
            ApplyAttachments(Root);
        }

        public void ApplyAttachments(VectorSpriteItem item)
        {
            foreach (IVectorSpriteItemAttachment attachment in _attachmentList)
            {
                attachment.Attach(item);
            }
            foreach (VectorSpriteItem child in item.Children)
            {
                ApplyAttachments(child);
            }
        }

    }
}
