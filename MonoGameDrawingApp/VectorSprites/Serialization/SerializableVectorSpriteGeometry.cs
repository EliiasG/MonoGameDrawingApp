using System.Linq;
using System.Numerics;

namespace MonoGameDrawingApp.VectorSprites.Serialization
{
    public class SerializableVectorSpriteGeometry
    {
        public SerializableVectorSpriteGeometry() { }
        public SerializableVectorSpriteGeometry(VectorSpriteGeometry geometry)
        {
            Points = geometry.Points.ToArray().Select((Vector2 v) => new SerializablleVector2(v)).ToArray();
            Color = new SerializableColor(geometry.Color);
        }

        public SerializablleVector2[] Points { get; set; }

        public SerializableColor Color { get; set; }

        public VectorSpriteGeometry ToGeometry()
        {
            return new VectorSpriteGeometry(Points.Select((SerializablleVector2 v) => v.ToVector()))
            {
                Color = Color.ToColor()
            };
        }
    }
}
