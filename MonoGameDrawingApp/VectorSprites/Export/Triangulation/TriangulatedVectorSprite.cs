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
            _triangulator = IPolygonTriangulator.CreateDefault();
            List<Vector2> vertices = new();
            List<int> indices = new();
            List<Color> colors = new();

            AddSpriteItem(vertices, indices, colors, sprite.Root);

            Vertices = vertices.ToArray();
            Indices = indices.ToArray();
            Colors = colors.ToArray();
        }

        public Vector2[] Vertices { get; }

        public int[] Indices { get; }

        public Color[] Colors { get; }

        private void AddSpriteItem(List<Vector2> vertices, List<int> indices, List<Color> colors, VectorSpriteItem item)
        {
            ModifiedGeometry modifiedGeometry = new(item.Geometry);

            TriangulatedGeometry geometry = new(_triangulator, modifiedGeometry);

            if (item.IsVisible)
            {
                foreach (TriangulatedPolygon polygon in geometry.Polygons)
                {
                    indices.AddRange(polygon.Indices.Select((int index) => index + vertices.Count));
                    vertices.AddRange(polygon.Vertices.Select((Vector2 vertex) => vertex + item.AbsolutePosition));
                    colors.AddRange(Enumerable.Repeat(polygon.Color, polygon.Vertices.Length));
                }
            }

            foreach (VectorSpriteItem child in item.Children.Reverse())
            {
                AddSpriteItem(vertices, indices, colors, child);
            }
        }
    }
}
