using System;
using System.Numerics;

namespace MonoGameDrawingApp
{
    public static class ExtraMath
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

        public static Vector2 Mirrored(Vector2 line1, Vector2 line2, Vector2 point)
        {
            Vector2 line = line2 - line1;
            point -= line1;

            float a1 = MathF.Atan2(point.Y, point.X);
            float a2 = MathF.Atan2(line.Y, line.X);

            float a3 = a2 * 2 - a1;

            return new Vector2(MathF.Cos(a3), MathF.Sin(a3)) * point.Length() + line1;
        }

        public static bool IsCounterClockwise(Vector2[] vertices)
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

            Vector2 prev = Util.GetItemCircled(vertices, lowestIndex - 1);
            Vector2 current = vertices[lowestIndex];
            Vector2 next = Util.GetItemCircled(vertices, lowestIndex + 1);

            Vector2 prevCurrent = current - prev;
            Vector2 nextCurrent = current - next;

            return prevCurrent.X * nextCurrent.Y < prevCurrent.Y * nextCurrent.X;
        }

        public static Vector2[] Expand(Vector2[] vertices, float amount)
        {
            bool inv = IsCounterClockwise(vertices);
            Vector2[] newVertices = new Vector2[vertices.Length];

            for (int i = 0; i < vertices.Length; i++)
            {
                Vector2 prev = Util.GetItemCircled(vertices, i - 1);
                Vector2 cur = vertices[i];
                Vector2 next = Util.GetItemCircled(vertices, i + 1);

                newVertices[i] = cur + GetExpanded(prev, cur, next, inv) * amount;
            }

            return newVertices;
        }


        public static Vector2 GetExpanded(Vector2 a, Vector2 b, Vector2 c, bool invert)
        {
            Vector2 ba = a - b;
            Vector2 bc = c - b;

            Vector2 baBack = -Vector2.Normalize(ba);
            Vector2 bcBack = -Vector2.Normalize(bc);

            Vector2 point;

            if (baBack == bcBack || baBack == -bcBack)
            {
                float angle = MathF.Atan2(ba.Y, ba.X) - MathF.PI / 2;
                point = new(MathF.Cos(angle), MathF.Sin(angle));
            }
            else
            {
                point = baBack + bcBack;

                float bcAngle = MathF.Atan2(bc.Y, bc.X);
                float baBackAngle = MathF.Atan2(baBack.Y, baBack.X);
                float angle = baBackAngle - bcAngle;

                //point = Vector2.Normalize(point);
                point /= MathF.Sin(angle);
                //point = bcBack;
            }
            //point += b;

            return invert ? -point : point;
        }

        public static Vector2 GetNormal(Vector2 a, Vector2 b, Vector2 c, bool counterClockwise)
        {
            Vector2 cb = b - c;
            Vector2 ab = b - a;

            float dot = cb.X * ab.Y - cb.Y * ab.X;

            float angle;

            Vector2 ba = a - b;
            Vector2 bc = c - b;

            bool invert;
            if (dot == 0)
            {
                angle = MathF.Atan2(ba.Y, ba.X) - MathF.PI / 2;
                invert = !counterClockwise;
            }
            else
            {
                float abAngle = MathF.Atan2(ba.Y, ba.X);
                float cbAngle = MathF.Atan2(bc.Y, bc.X);

                abAngle = (abAngle < 0 && MathF.Abs(abAngle - cbAngle) > MathF.PI) ? abAngle + MathF.Tau : abAngle;
                cbAngle = (cbAngle < 0 && MathF.Abs(cbAngle - abAngle) > MathF.PI) ? cbAngle + MathF.Tau : cbAngle;

                angle = (abAngle + cbAngle) * 0.5f;

                if (counterClockwise)
                {
                    invert = dot < 0;
                }
                else
                {
                    invert = dot > 0;
                }
            }

            float cos = MathF.Cos(angle);
            float sin = MathF.Sin(angle);

            Vector2 res = new(cos, sin);// * (MathF.Abs(cos) + MathF.Abs(sin));

            return invert ? res : -res;
        }
    }
}
