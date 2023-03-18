using MonoGameDrawingApp.VectorSprites.Modifiers;
using MonoGameDrawingApp.VectorSprites.Serialization;
using System.Linq;
using System.Numerics;

namespace MonoGameDrawingApp.VectorSprites.Attachments.UndoRedo.Snapshots
{
    public class VectorSpriteItemDataSnapshot
    {
        public VectorSpriteItemDataSnapshot(VectorSpriteItem item)
        {
            Position = item.Position;
            Name = item.Name;
            Geometry = new VectorSpriteGeometry(item.Geometry.Points)
            {
                Color = item.Geometry.Color
            };
            Modifiers = item.Modifiers.Select(
                (IGeometryModifier modifier) =>
                new SerializableGeometryModifier(modifier)
            ).ToArray();
        }

        public Vector2 Position { get; set; }

        public string Name { get; set; }

        public VectorSpriteGeometry Geometry { get; set; }

        public SerializableGeometryModifier[] Modifiers { get; set; }

        public void Apply(VectorSpriteItem item)
        {
            item.Position = Position;
            item.Name = Name;
            item.Geometry.Points = Geometry.Points;
            item.Geometry.Color = Geometry.Color;
            item.Modifiers = Modifiers.Select((SerializableGeometryModifier modifier) => modifier.ToModifier(item.Sprite));
        }
    }
}
