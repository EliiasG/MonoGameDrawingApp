using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;

namespace MonoGameDrawingApp.VectorSprites.Export.Triangulation
{
    public class TriangulatedVectorSprite
    {
        private readonly IPolygonTriangulator _triangulator;

        public TriangulatedVectorSprite(VectorSprite sprite)
        {
            _triangulator = new EarClippingGeometryTriangulator();
            List<Vector2> vertices = new();
            List<int> indices = new();
            List<Color> colors = new();

            _addSpriteItem(vertices, indices, colors, sprite.Root);

            Vertices = vertices.ToArray();
            Indices = indices.ToArray();
            Colors = colors.ToArray();
        }

        public Vector2[] Vertices { get; }

        public int[] Indices { get; }

        public Color[] Colors { get; }

        private void _addSpriteItem(List<Vector2> vertices, List<int> indices, List<Color> colors, VectorSpriteItem item)
        {
            TriangulatedGeometry geometry = new(_triangulator, new ModifiedGeometry(item.Geometry));

            foreach (TriangulatedPolygon polygon in geometry.Polygons)
            {
                indices.AddRange(polygon.Indices.Select((int index) => index + vertices.Count));
                vertices.AddRange(polygon.Vertices.Select((Vector2 vertex) => vertex + item.AbsolutePosition));
                colors.AddRange(Enumerable.Repeat(polygon.Color, polygon.Vertices.Length));
            }

            foreach (VectorSpriteItem child in item.Children.Reverse())
            {
                _addSpriteItem(vertices, indices, colors, child);
            }
        }
    }
}
