using MonoGameDrawingApp.VectorSprites.Export;
using MonoGameDrawingApp.VectorSprites.Modifiers.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace MonoGameDrawingApp.VectorSprites.Modifiers.Appliable.Simple
{
    public class RoundModifier : SimpleModifier
    {
        private readonly GeometryModifierParameter<float> _amount;
        private readonly GeometryModifierParameter<int> _resolution;

        public RoundModifier()
        {
            _amount = new GeometryModifierParameter<float>(0, "Amount");
            _resolution = new GeometryModifierParameter<int>(1, "Resolution", (int v) => Math.Clamp(v, 1, 8));
            Parameters = new IGeometryModifierParameter[]
            {
                _amount,
                _resolution,
            };
        }

        public override string Name => "Round";

        public override IEnumerable<IGeometryModifierParameter> Parameters { get; }

        protected override Polygon ModifyPolygon(Polygon polygon)
        {
            Vector2[] points = polygon.Vertices.ToArray();

            float amount = _amount.Value;
            int resolution = _resolution.Value;

            if (resolution < 1 || amount <= 0)
            {
                return polygon;
            }

            float amt = amount;

            if (resolution > 1)
            {
                double pow = Math.Pow(2, resolution - 1);
                amt = amount / (1 + ((float)((pow - 1) / pow) / 2));
            }

            for (int i = 0; i < resolution; i++)
            {
                Vector2[] newPoints = new Vector2[points.Length * 2];
                for (int j = 0; j < points.Length; j++)
                {
                    Vector2 prev = Util.GetItemCircled(points, j - 1);
                    Vector2 cur = Util.GetItemCircled(points, j);
                    Vector2 next = Util.GetItemCircled(points, j + 1);

                    newPoints[j * 2] = Vector2.Lerp(cur, prev, amt / Vector2.Distance(cur, prev));
                    newPoints[(j * 2) + 1] = Vector2.Lerp(cur, next, amt / Vector2.Distance(cur, next));
                }
                points = newPoints;
                amt /= 4;
            }

            return new Polygon(points, polygon.Color);
        }
    }
}
