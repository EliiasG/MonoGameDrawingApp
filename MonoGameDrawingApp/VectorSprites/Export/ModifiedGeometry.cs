using System.Collections.Generic;

namespace MonoGameDrawingApp.VectorSprites.Export
{
    public class ModifiedGeometry
    {
        public ModifiedGeometry(VectorSpriteGeometry geometry)
        {
            ModifiedPolygons = new List<Polygon>() { geometry.ToPolygon() };

            foreach (IVectorSpriteItemModifier modifier in geometry.VectorSpriteItem.Modifiers)
            {
                modifier.Apply(this);
            }
        }
        public List<Polygon> ModifiedPolygons { get; init; }
    }
}
