using MonoGameDrawingApp.VectorSprites.Export;
using MonoGameDrawingApp.VectorSprites.Modifiers.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace MonoGameDrawingApp.VectorSprites.Modifiers.Appliable.Simple
{
    public class RandomizeModifier : SimpleModifier
    {
        private readonly GeometryModifierParameter<Vector2> _amount;
        private readonly GeometryModifierParameter<int> _seed;

        public RandomizeModifier()
        {
            _amount = new GeometryModifierParameter<Vector2>(Vector2.Zero, "Amount");
            _seed = new GeometryModifierParameter<int>(0, "Seed");

            Parameters = new IGeometryModifierParameter[]
            {
                _seed,
                _amount,
            };
        }

        public override string Name => "Randomize";

        public override IEnumerable<IGeometryModifierParameter> Parameters { get; }

        protected override Polygon _modifyPolygon(Polygon polygon)
        {
            Vector2[] vertices = polygon.Vertices.ToArray();
            Random rnd = new(_seed.Value);

            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] = vertices[i] + new Vector2((rnd.NextSingle() - 0.5f) * 2, (rnd.NextSingle() - 0.5f) * 2) * _amount.Value;
            }

            return new Polygon(vertices, polygon.Color);
        }
    }
}
