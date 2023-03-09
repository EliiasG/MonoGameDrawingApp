using System.Collections.Generic;

namespace MonoGameDrawingApp.VectorSprites.Attachments.UndoRedo.Snapshots
{
    public class VectorSpriteItemChildrenSnapshot
    {
        public VectorSpriteItemChildrenSnapshot(VectorSpriteItem item)
        {
            Children = new List<VectorSpriteItem>(item.Children);
        }

        public IEnumerable<VectorSpriteItem> Children { get; init; }

        public void Apply(VectorSpriteItem item)
        {
            item.Children = Children;
        }
    }
}
