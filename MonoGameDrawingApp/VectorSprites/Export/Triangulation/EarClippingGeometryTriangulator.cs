using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace MonoGameDrawingApp.VectorSprites.Export.Triangulation
{
    public class EarClippingGeometryTriangulator : IPolygonTriangulator
    {

        public TriangulatedPolygon Triangulate(Polygon polygon)
        {
            try
            {
                return _triangulate(polygon);
            }
            catch
            {
                return new TriangulatedPolygon(Array.Empty<Vector2>(), Array.Empty<int>(), polygon.Color);
            }
        }
        private static TriangulatedPolygon _triangulate(Polygon polygon)
        {
            Vector2[] vertices = _withoutCollinear(polygon.Vertices);
            vertices = _makeCounterClockwise(vertices);

            if (vertices.Length < 3)
            {
                return new TriangulatedPolygon(Array.Empty<Vector2>(), Array.Empty<int>(), polygon.Color);
            }

            List<int> indices = new()
            {
                Capacity = (vertices.Length - 2) * 3
            };

            int remaning = 0;

            int?[] remaningIndices = Enumerable.Range(0, vertices.Length).Select((int n) => (int?)n).ToArray();

            while (remaning < vertices.Length - 3)
            {
                int oldRemaning = remaning;
                for (int i = 0; i < vertices.Length; i++)
                {
                    if (remaningIndices[i] == null)
                    {
                        continue;
                    }

                    int previousIndex = _findNotNull(remaningIndices, i - 1, -1);
                    int currentIndex = (int)remaningIndices[i];
                    int nextIndex = _findNotNull(remaningIndices, i + 1, 1);

                    Vector2 previous = vertices[previousIndex];
                    Vector2 current = vertices[currentIndex];
                    Vector2 next = vertices[nextIndex];

                    if (ExtraMath.IsOnLine(current, previous, next))
                    {
                        remaningIndices[i] = null;

                        ++remaning;

                        if (remaning >= vertices.Length - 3)
                        {
                            break;
                        }
                    }

                    if (!ExtraMath.IsConvex(previous, current, next))
                    {
                        continue;
                    }

                    if (_anyInTriangle(vertices, previous, current, next))
                    {
                        continue;
                    }

                    remaningIndices[i] = null;

                    indices.Add(nextIndex);
                    indices.Add(currentIndex);
                    indices.Add(previousIndex);

                    ++remaning;

                    if (remaning >= vertices.Length - 3)
                    {
                        break;
                    }
                }

                if (remaning == oldRemaning)
                {
                    return new TriangulatedPolygon(Array.Empty<Vector2>(), Array.Empty<int>(), polygon.Color);
                }
            }

            foreach (int? index in remaningIndices.Reverse())
            {
                if (index is int intIndex)
                {
                    indices.Add(intIndex);
                }
            }

            if (indices.Count % 3 == 0)
            {
                return new TriangulatedPolygon(vertices, indices.ToArray(), polygon.Color);
            }
            return new TriangulatedPolygon(Array.Empty<Vector2>(), Array.Empty<int>(), polygon.Color);
        }

        private static bool _anyInTriangle(Vector2[] points, Vector2 a, Vector2 b, Vector2 c)
        {
            foreach (Vector2 p in points)
            {
                if (p == a || p == b || p == c)
                {
                    continue;
                }
                if (_isInTrangle(p, a, b, c))
                {
                    return true;
                }
            }
            return false;
        }

        private static int _findNotNull(int?[] indices, int index, int move)
        {
            for (int i = 0; i < indices.Length; i++)
            {
                int? idx = Util.GetItemCircled(indices, index + i * move);
                if (idx is int intIdx)
                {
                    return intIdx;
                }
            }
            throw new ArgumentException("All values are null");
        }

        private static Vector2[] _withoutCollinear(Vector2[] vertices)
        {
            List<Vector2> newVertecies = null;

            for (int i = 0; i < vertices.Length; i++)
            {
                Vector2 last = _previousPoint(vertices, i);
                Vector2 current = vertices[i];
                Vector2 next = _nextPoint(vertices, i);

                if (ExtraMath.IsOnLine(current, last, next) && newVertecies == null)
                {
                    newVertecies ??= vertices.Take(i).ToList();
                }
                else
                {
                    newVertecies?.Add(current);
                }
            }

            return newVertecies?.ToArray() ?? vertices;
        }

        private static Vector2[] _makeCounterClockwise(Vector2[] vertices)
        {
            return _isCounterClockwise(vertices) ? vertices : vertices.Reverse().ToArray();
        }

        private static Vector2 _previousPoint(Vector2[] points, int index)
        {
            return points[(index == 0 ? points.Length : index) - 1];
        }

        private static Vector2 _nextPoint(Vector2[] points, int index)
        {
            return points[index == points.Length - 1 ? 0 : (index + 1)];
        }

        private static bool _isCounterClockwise(Vector2[] vertices)
        {
            int lowestIndex = 0;
            for (int i = 0; i < vertices.Length; i++)
            {
                Vector2 lowestPoint = vertices[lowestIndex];
                Vector2 point = vertices[i];
                if (point.Y < lowestPoint.Y || (point.Y == lowestPoint.Y && point.X > lowestPoint.X))
                {
                    lowestIndex = i;
                }
            }

            Vector2 last = _previousPoint(vertices, lowestIndex);
            Vector2 current = vertices[lowestIndex];
            Vector2 next = _nextPoint(vertices, lowestIndex);

            Vector2 lastCurrent = current - last;
            Vector2 nextCurrent = current - next;

            return lastCurrent.X * nextCurrent.Y < lastCurrent.Y * nextCurrent.X;
        }

        private static bool _isInTrangle(Vector2 point, Vector2 a, Vector2 b, Vector2 c)
        {
            bool b1 = _isCounterClockwise(new Vector2[] { a, b, point });
            bool b2 = _isCounterClockwise(new Vector2[] { b, c, point });
            bool b3 = _isCounterClockwise(new Vector2[] { c, a, point });
            return (b1 == b2 && b2 == b3) || ExtraMath.IsOnLine(point, a, b) || ExtraMath.IsOnLine(point, a, c) || ExtraMath.IsOnLine(point, b, c);
        }
    }
}
