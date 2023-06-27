using MonoGameDrawingApp.VectorSprites.Export;
using MonoGameDrawingApp.VectorSprites.Modifiers.Parameters;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace MonoGameDrawingApp.VectorSprites.Modifiers.Appliable.Simple
{
    public class FlipModifier : SimpleModifier
    {
        private readonly GeometryModifierParameter<bool> _horizontal;
        private readonly GeometryModifierParameter<bool> _vertical;

        public FlipModifier()
        {
            _horizontal = new GeometryModifierParameter<bool>(false, "Horizontal");
            _vertical = new GeometryModifierParameter<bool>(false, "Vertical");
            Parameters = new IGeometryModifierParameter[]
            {
                _horizontal,
                _vertical,
            };
        }

        public override string Name => "Flip";

        public override IEnumerable<IGeometryModifierParameter> Parameters { get; }

        protected override Polygon ModifyPolygon(Polygon polygon)
        {
            Vector2[] vertices = polygon.Vertices.ToArray();

            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] = vertices[i] * new Vector2(_horizontal.Value ? -1 : 1, _vertical.Value ? -1 : 1);
            }

            return new Polygon(vertices, polygon.Color);
        }
    }
}
