using MonoGameDrawingApp.VectorSprites.Export;
using MonoGameDrawingApp.VectorSprites.Modifiers.Parameters;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace MonoGameDrawingApp.VectorSprites.Modifiers.Appliable.Simple
{
    public class MoveModifier : SimpleModifier
    {
        private readonly GeometryModifierParameter<Vector2> _offset;

        public MoveModifier()
        {
            _offset = new GeometryModifierParameter<Vector2>(Vector2.Zero, "Offset");
            Parameters = new IGeometryModifierParameter[]
            {
                _offset
            };
        }

        public override string Name => "Move";

        public override IEnumerable<IGeometryModifierParameter> Parameters { get; }

        protected override Polygon _modifyPolygon(Polygon polygon)
        {
            Vector2[] points = polygon.Vertices.ToArray();
            for (int i = 0; i < points.Length; i++)
            {
                points[i] = points[i] + _offset.Value;
            }
            return new Polygon(points, polygon.Color);
        }
    }
}
