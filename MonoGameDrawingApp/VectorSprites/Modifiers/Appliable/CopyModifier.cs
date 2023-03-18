using MonoGameDrawingApp.VectorSprites.Export;
using MonoGameDrawingApp.VectorSprites.Modifiers.Parameters;
using System.Collections.Generic;
using System.Linq;

namespace MonoGameDrawingApp.VectorSprites.Modifiers.Appliable
{
    public class CopyModifier : IAppliableGeometryModifier
    {
        public string Name => "Copy";

        private readonly GeometryModifierParameter<VectorSpriteItemReference> _from;

        public CopyModifier()
        {
            _from = new GeometryModifierParameter<VectorSpriteItemReference>(new VectorSpriteItemReference(), "From");
            Parameters = new IGeometryModifierParameter[]
            {
                _from,
            };
        }

        public IEnumerable<IGeometryModifierParameter> Parameters { get; }

        public void Apply(VectorSpriteItem item)
        {
            if (_from.Value.Item != null)
            {
                item.Geometry.Points = _from.Value.Item.Geometry.Points;
                item.Geometry.Color = _from.Value.Item.Geometry.Color;
                item.Modifiers = _from.Value.Item.Modifiers.Concat(item.Modifiers);
            }
        }

        public void Modify(ModifiedGeometry geometry)
        {
            ModifiedGeometry modifiedGeometry = _from.Value.ModifiedGeometry;
            if (modifiedGeometry != null)
            {
                geometry.ModifiedPolygons.Clear();
                geometry.ModifiedPolygons.AddRange(modifiedGeometry.ModifiedPolygons);
            }
        }
    }
}
