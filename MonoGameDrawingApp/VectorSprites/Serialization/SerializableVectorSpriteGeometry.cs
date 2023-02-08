using System.Drawing;
using System.Linq;
using System.Numerics;

namespace MonoGameDrawingApp.VectorSprites.Serialization
{
    public class SerializableVectorSpriteGeometry
    {
        public SerializableVectorSpriteGeometry(VectorSpriteGeometry geometry)
        {
            Points = geometry.Points.ToArray();
            Color = geometry.Color;
        }

        public Vector2[] Points { get; set; }

        public Color Color { get; set; }

        public VectorSpriteGeometry ToGeometry()
        {
            return new VectorSpriteGeometry(Points)
            {
                Color = Color
            };
        }
    }
}
