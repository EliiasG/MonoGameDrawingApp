using MonoGameDrawingApp.VectorSprites.Export;
using MonoGameDrawingApp.VectorSprites.Modifiers.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace MonoGameDrawingApp.VectorSprites.Modifiers.Appliable.Simple
{
    public class RotateModifier : SimpleModifier
    {
        private readonly GeometryModifierParameter<float> _degrees;

        public RotateModifier()
        {
            _degrees = new GeometryModifierParameter<float>(0, "Degrees");

            Parameters = new IGeometryModifierParameter[]
            {
                _degrees,
            };
        }

        public override string Name => "Rotate";

        public override IEnumerable<IGeometryModifierParameter> Parameters { get; }

        protected override Polygon _modifyPolygon(Polygon polygon)
        {
            Vector2[] vertices = polygon.Vertices.ToArray();
            float angle = -_degrees.Value * MathF.PI / 180;


            for (int i = 0; i < vertices.Length; i++)
            {
                Vector2 vertex = vertices[i];
                float length = vertex.Length();
                float vertexAngle = MathF.Atan2(vertex.Y, vertex.X) + angle;
                vertices[i] = new Vector2(MathF.Cos(vertexAngle), MathF.Sin(vertexAngle)) * length;
            }

            return new Polygon(vertices, polygon.Color);
        }
    }
}
