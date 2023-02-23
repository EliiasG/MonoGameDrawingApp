using System.Linq;

namespace MonoGameDrawingApp.VectorSprites.Export.Triangulation
{
    public class TriangulatedGeometry
    {
        public TriangulatedGeometry(IPolygonTriangulator triangulator, ModifiedGeometry modifiedGeometry)
        {
            Polygons = modifiedGeometry.ModifiedPolygons.Select((Polygon p) => triangulator.Triangulate(p)).ToArray();
        }

        public TriangulatedPolygon[] Polygons { get; init; }

    }
}
