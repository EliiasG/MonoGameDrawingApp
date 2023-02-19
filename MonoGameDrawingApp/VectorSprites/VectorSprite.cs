using MonoGameDrawingApp.VectorSprites.Attachments;
using System.Collections.Generic;

namespace MonoGameDrawingApp.VectorSprites
{
    public class VectorSprite
    {
        private readonly List<IVectorSpriteAttachment> _attachmentList;

        public VectorSprite()
        {
            _attachmentList = new List<IVectorSpriteAttachment>();
        }

        public VectorSpriteItem Root { get; set; }

        public void AddAttachment(IVectorSpriteAttachment attachment)
        {
            _attachmentList.Add(attachment);
            ApplyAttachments(Root);
        }

        public void ApplyAttachments(VectorSpriteItem item)
        {
            foreach (IVectorSpriteAttachment attachment in _attachmentList)
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
