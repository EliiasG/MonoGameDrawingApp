using MonoGameDrawingApp.VectorSprites.Modifiers;
using System.Collections.Generic;
using System.Numerics;

namespace MonoGameDrawingApp.VectorSprites.Export
{
    public class ModifiedGeometry
    {
        public ModifiedGeometry(VectorSpriteGeometry geometry)
        {
            ModifiedPolygons = geometry.Item.IsVisible ? new List<Polygon>() { geometry.ToPolygon() } : new List<Polygon>();

            Position = geometry.Item.AbsolutePosition;

            if (!geometry.Item.IsVisible)
            {
                return;
            }

            foreach (IGeometryModifier modifier in geometry.Item.Modifiers)
            {
                try
                {
                    modifier.Modify(this);
                }
                catch { }
            }
        }

        public List<Polygon> ModifiedPolygons { get; init; }

        public Vector2 Position { get; }
    }
}
