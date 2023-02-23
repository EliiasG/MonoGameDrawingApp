using System.Drawing;
using System.Numerics;

namespace MonoGameDrawingApp.VectorSprites.Export
{
    public class Polygon
    {
        public Polygon(Vector2[] vertices, Color color)
        {
            Vertices = vertices;
            Color = color;
        }

        public Vector2[] Vertices { get; init; }
        public Color Color { get; init; }
    }
}
