using System;
using System.Numerics;

namespace MonoGameDrawingApp
{
    public class ExtraMath
    {
        public static bool IsOnLine(Vector2 point, Vector2 lineStart, Vector2 lineEnd)
        {
            Vector2 line = lineEnd - lineStart;
            Vector2 pointToEnd = lineEnd - point;

            if (Math.Sign(line.X) != Math.Sign(pointToEnd.X))
            {
                return false;
            }

            if (Math.Sign(line.Y) != Math.Sign(pointToEnd.Y))
            {
                return false;
            }

            if (pointToEnd.LengthSquared() > line.LengthSquared())
            {
                return false;
            }

            if (line.Y == 0)
            {
                return pointToEnd.Y == 0;
            }

            if (pointToEnd.Y == 0)
            {
                return false;
            }

            return line.X / line.Y == pointToEnd.X / pointToEnd.Y;
        }

        public static bool IsConvex(Vector2 a, Vector2 b, Vector2 c)
        {
            Vector2 cb = b - c;
            Vector2 ab = b - a;
            return cb.X * ab.Y > cb.Y * ab.X;
        }

        public static Vector2 Project(Vector2 u, Vector2 v)
        {
            return Vector2.Dot(u, v) * v.LengthSquared() * v;
        }

        public static Vector2 ClosestPointOnLine(Vector2 point, Vector2 a, Vector2 b)
        {
            Vector2 aToPoint = point - a;
            Vector2 aToB = b - a;

            float lengthSquared = aToB.LengthSquared();
            float dot = Vector2.Dot(aToPoint, aToB);
            float distance = dot / lengthSquared;

            if (distance < 0)
            {
                return a;

            }
            else if (distance > 1)
            {
                return b;
            }
            else
            {
                return a + aToB * distance;
            }
        }
    }
}
