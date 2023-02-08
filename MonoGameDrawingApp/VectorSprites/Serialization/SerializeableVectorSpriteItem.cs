using System.Linq;
using System.Numerics;

namespace MonoGameDrawingApp.VectorSprites.Serialization
{
    public class SerializeableVectorSpriteItem
    {
        public SerializeableVectorSpriteItem(VectorSpriteItem item)
        {
            Name = item.Name;
            Position = item.Position;
            Geometry = new SerializableVectorSpriteGeometry(item.Geometry);

            Children = new SerializeableVectorSpriteItem[item.Children.Count()];

            int index = 0;
            foreach (VectorSpriteItem child in item.Children)
            {
                Children[index] = new SerializeableVectorSpriteItem(child);
                ++index;
            }
        }

        public string Name { get; set; }

        public Vector2 Position { get; set; }

        public SerializableVectorSpriteGeometry Geometry { get; set; }

        public SerializeableVectorSpriteItem[] Children { get; set; }

        //TODO modifiers

        public VectorSpriteItem ToSpriteItem(VectorSprite sprite)
        {
            VectorSpriteItem res = new(Name, sprite, Geometry.ToGeometry(), Position);
            foreach (SerializeableVectorSpriteItem child in Children)
            {
                res.AddChild(child.ToSpriteItem(sprite));
            }
            return res;
        }
    }
}
