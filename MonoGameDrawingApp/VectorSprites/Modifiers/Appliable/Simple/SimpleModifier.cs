using MonoGameDrawingApp.VectorSprites.Export;
using MonoGameDrawingApp.VectorSprites.Modifiers.Parameters;
using System.Collections.Generic;
using System.Linq;

namespace MonoGameDrawingApp.VectorSprites.Modifiers.Appliable.Simple
{
    public abstract class SimpleModifier : IAppliableGeometryModifier
    {
        public abstract string Name { get; }


        public abstract IEnumerable<IGeometryModifierParameter> Parameters { get; }

        public void Apply(VectorSpriteItem item)
        {
            Polygon polygon = _modifyPolygon(new Polygon(item.Geometry.Points.ToArray(), item.Geometry.Color));
            item.Geometry.Points = polygon.Vertices;
            item.Geometry.Color = polygon.Color;
        }

        public void Modify(ModifiedGeometry geometry)
        {
            _start();
            for (int i = 0; i < geometry.ModifiedPolygons.Count; i++)
            {
                geometry.ModifiedPolygons[i] = _modifyPolygon(geometry.ModifiedPolygons[i]);
            }
        }

        protected virtual void _start() { }

        protected abstract Polygon _modifyPolygon(Polygon polygon);
    }
}
