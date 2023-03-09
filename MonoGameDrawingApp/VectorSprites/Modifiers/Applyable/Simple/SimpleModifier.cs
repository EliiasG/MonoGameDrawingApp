using MonoGameDrawingApp.VectorSprites.Export;
using System;
using System.Linq;

namespace MonoGameDrawingApp.VectorSprites.Modifiers.Applyable.Simple
{
    public abstract class SimpleModifier : IApplyableVectorSpriteItemModifier
    {
        public abstract Action Changed { get; set; }
        public abstract Action Changing { get; set; }

        public void Apply(VectorSpriteItem item)
        {
            Polygon polygon = _modifyPolygon(new Polygon(item.Geometry.Points.ToArray(), item.Geometry.Color));
            item.Geometry.Points = polygon.Vertices;
            item.Geometry.Color = polygon.Color;
        }

        public void Modify(ModifiedGeometry geometry)
        {
            geometry.ModifiedPolygons[0] = _modifyPolygon(geometry.ModifiedPolygons[0]);
        }

        protected virtual void _start() { }

        protected abstract Polygon _modifyPolygon(Polygon polygon);
    }
}
