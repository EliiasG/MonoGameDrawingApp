using MonoGameDrawingApp.VectorSprites.Export;
using MonoGameDrawingApp.VectorSprites.Modifiers.Parameters;
using System.Collections.Generic;

namespace MonoGameDrawingApp.VectorSprites.Modifiers.Appliable.Simple
{
    public class ExpandModifier : SimpleModifier
    {
        private readonly GeometryModifierParameter<float> _amount;

        public override string Name => "Expand";

        public ExpandModifier() : this(0)
        {
        }

        public ExpandModifier(float amount)
        {
            _amount = new GeometryModifierParameter<float>(amount, "Amount");
            Parameters = new IGeometryModifierParameter[]
            {
                _amount,
            };
        }

        public override IEnumerable<IGeometryModifierParameter> Parameters { get; }

        protected override Polygon ModifyPolygon(Polygon polygon)
        {
            if (polygon.Vertices.Length < 3)
            {
                return new Polygon(polygon.Vertices, polygon.Color);
            }
            return new Polygon(ExtraMath.Expand(polygon.Vertices, _amount.Value), polygon.Color);
        }
    }
}
