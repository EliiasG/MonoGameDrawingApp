using System.Collections.Generic;
using MonoGameDrawingApp.VectorSprites.Modifiers;

namespace MonoGameDrawingApp.VectorSprites.Export
{
    public class ModifiedGeometry
    {
        public ModifiedGeometry(VectorSpriteGeometry geometry)
        {
            ModifiedPolygons = new List<Polygon>() { geometry.ToPolygon() };

            foreach (IGeometryModifier modifier in geometry.VectorSpriteItem.Modifiers)
            {
                modifier.Modify(this);
            }
        }
        public List<Polygon> ModifiedPolygons { get; init; }
    }
}
