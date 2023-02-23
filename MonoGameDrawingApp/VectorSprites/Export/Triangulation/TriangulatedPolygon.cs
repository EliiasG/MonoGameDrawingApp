using System.Drawing;
using System.Numerics;

namespace MonoGameDrawingApp.VectorSprites.Export.Triangulation
{
    public class TriangulatedPolygon
    {
        public TriangulatedPolygon(Vector2[] vertices, int[] indices, Color color)
        {
            Vertices = vertices;
            Indices = indices;
            Color = color;
        }

        public Vector2[] Vertices { get; init; }
        public int[] Indices { get; init; }
        public Color Color { get; init; }
    }
}
