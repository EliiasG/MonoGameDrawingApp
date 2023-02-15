using System.Linq;

namespace MonoGameDrawingApp.VectorSprites.Serialization
{
    public class SerializableVectorSpriteItem
    {
        public SerializableVectorSpriteItem(VectorSpriteItem item)
        {
            Name = item.Name;
            Position = new SerializablleVector2(item.Position);
            Geometry = new SerializableVectorSpriteGeometry(item.Geometry);

            Children = new SerializableVectorSpriteItem[item.Children.Count()];

            int index = 0;
            foreach (VectorSpriteItem child in item.Children)
            {
                Children[index] = new SerializableVectorSpriteItem(child);
                ++index;
            }
        }

        public string Name { get; set; }

        public SerializablleVector2 Position { get; set; }

        public SerializableVectorSpriteGeometry Geometry { get; set; }

        public SerializableVectorSpriteItem[] Children { get; set; }

        //TODO modifiers

        public VectorSpriteItem ToSpriteItem(VectorSprite sprite)
        {
            VectorSpriteItem res = new(Name, sprite, Geometry.ToGeometry(), Position.ToVector());
            foreach (SerializableVectorSpriteItem child in Children)
            {
                res.AddChild(child.ToSpriteItem(sprite));
            }
            return res;
        }
    }
}
