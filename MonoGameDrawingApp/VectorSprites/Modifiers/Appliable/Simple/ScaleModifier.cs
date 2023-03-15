using MonoGameDrawingApp.VectorSprites.Export;
using MonoGameDrawingApp.VectorSprites.Modifiers.Parameters;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace MonoGameDrawingApp.VectorSprites.Modifiers.Appliable.Simple
{
    public class ScaleModifier : SimpleModifier
    {
        private readonly GeometryModifierParameter<Vector2> _size;

        public ScaleModifier()
        {
            _size = new GeometryModifierParameter<Vector2>(Vector2.One, "Size");
            Parameters = new IGeometryModifierParameter[]
            {
                _size,
            };
        }

        public override string Name => "Scale";

        public override IEnumerable<IGeometryModifierParameter> Parameters { get; }

        protected override Polygon _modifyPolygon(Polygon polygon)
        {
            Vector2[] vertices = polygon.Vertices.ToArray();

            for (int i = 0; i < polygon.Vertices.Length; i++)
            {
                vertices[i] = vertices[i] * _size.Value;
            }

            return new Polygon(vertices, polygon.Color);
        }
    }
}
