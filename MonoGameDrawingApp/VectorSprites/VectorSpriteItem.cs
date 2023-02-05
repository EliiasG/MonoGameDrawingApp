using System.Collections.Generic;

namespace MonoGameDrawingApp.VectorSprites
{
    public class VectorSpriteItem
    {
        private readonly List<VectorSpriteItem> _children;

        private readonly List<IVectorSpriteItemModifier> _modifiers;

        public VectorSpriteItem(string name)
        {
            _children = new List<VectorSpriteItem>();
            _modifiers = new List<IVectorSpriteItemModifier>();

            Name = name;
        }

        public string Name { get; set; }

        public VectorSpriteGeometry Geometry { get; set; }

        public VectorSpriteItem Parent { get; private set; }

        public IEnumerable<VectorSpriteItem> Children => _children;

        public IEnumerable<IVectorSpriteItemModifier> Modifiers => _modifiers;

        public void RemoveChild(VectorSpriteItem child)
        {
            _children.Remove(child);
            child.Parent = null;
        }

        public void AddChild(VectorSpriteItem child)
        {
            child.Parent?.RemoveChild(child);
            _children.Add(child);
            child.Parent = this;
        }

    }
}
