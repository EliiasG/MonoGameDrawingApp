using MonoGameDrawingApp.VectorSprites.Modifiers;
using System.Collections.Generic;
using System.Numerics;

namespace MonoGameDrawingApp.VectorSprites.Export
{
    public class ModifiedGeometry
    {
        public ModifiedGeometry(VectorSpriteGeometry geometry)
        {
            ModifiedPolygons = new List<Polygon>() { geometry.ToPolygon() };

            Position = geometry.Item.AbsolutePosition;

            foreach (IGeometryModifier modifier in geometry.Item.Modifiers)
            {
                modifier.Modify(this);
            }
        }

        public List<Polygon> ModifiedPolygons { get; init; }

        public Vector2 Position { get; }
    }
}
