using MonoGameDrawingApp.VectorSprites.Modifiers;
using MonoGameDrawingApp.VectorSprites.Serialization.Modifiers;
using System.Linq;

namespace MonoGameDrawingApp.VectorSprites.Serialization
{
    public class SerializableVectorSpriteItem
    {
        public SerializableVectorSpriteItem() { }

        public SerializableVectorSpriteItem(VectorSpriteItem item, bool addChildren = true)
        {
            Name = item.Name;
            Position = new SerializablleVector2(item.Position);
            Geometry = new SerializableVectorSpriteGeometry(item.Geometry);

            if (!addChildren)
            {
                Children = new SerializableVectorSpriteItem[item.Children.Count()];
            }
            else
            {
                Children = item.Children.Select(
                    (VectorSpriteItem child) =>
                    new SerializableVectorSpriteItem(child, addChildren)
                ).ToArray();
            }

            Modifiers = item.Modifiers.Select(
                (IVectorSpriteItemModifier modifier) =>
                VectorSpriteItemModifierSerializer.Serialize(modifier)
            ).ToArray();

        }

        public string Name { get; set; }

        public SerializablleVector2 Position { get; set; }

        public SerializableVectorSpriteGeometry Geometry { get; set; }

        public ISerializableVectorSpriteItemModifier[] Modifiers { get; set; }

        public SerializableVectorSpriteItem[] Children { get; set; }

        public VectorSpriteItem ToSpriteItem(VectorSprite sprite)
        {
            VectorSpriteItem res = new(Name, sprite, Geometry.ToGeometry(), Position.ToVector());
            foreach (SerializableVectorSpriteItem child in Children)
            {
                res.AddChild(child.ToSpriteItem(sprite));
            }
            foreach (ISerializableVectorSpriteItemModifier modifier in Modifiers)
            {
                res.AddModifier(modifier.ToModifier());
            }
            return res;
        }
    }
}
