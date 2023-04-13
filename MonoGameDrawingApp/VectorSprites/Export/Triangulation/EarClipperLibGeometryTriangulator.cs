using EarClipperLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;


namespace MonoGameDrawingApp.VectorSprites.Export.Triangulation
{
    public class EarClipperLibGeometryTriangulator : IPolygonTriangulator
    {
        public TriangulatedPolygon Triangulate(Polygon polygon)
        {
            EarClipping clipping = new();

            List<Vector3m> points = (from Vector2 point
                                     in ExtraMath.MakeCounterClockwise(polygon.Vertices)
                                     select new Vector3m(point.X, point.Y, 0)).ToList();

            clipping.SetPoints(points);

            try
            {
                clipping.Triangulate();
            }
            catch
            {
                return new TriangulatedPolygon(Array.Empty<Vector2>(), Array.Empty<int>(), polygon.Color);
            }

            List<Vector3m> result = clipping.Result;

            Dictionary<Vector2, ISet<int>> vertexPointers = new();

            for (int i = 0; i < result.Count; i++)
            {
                Vector3m point = result[i];
                // the rational class only allows convertion to double, so the weird (float)(double) is required
                Vector2 vertex = new((float)(double)point.X, (float)(double)point.Y);
                if (!vertexPointers.ContainsKey(vertex))
                {
                    vertexPointers[vertex] = new HashSet<int>();
                }
                vertexPointers[vertex].Add(i);
            }

            Vector2[] vertices = vertexPointers.Keys.ToArray();

            int[] indices = new int[result.Count];

            for (int i = 0; i < result.Count; i++)
            {
                for (int j = 0; j < vertices.Length; j++)
                {
                    if (vertexPointers[vertices[j]].Contains(i))
                    {
                        indices[indices.Length - i - 1] = j;
                        break;
                    }
                }
            }

            return new TriangulatedPolygon(vertices, indices, polygon.Color);
        }
    }
}
