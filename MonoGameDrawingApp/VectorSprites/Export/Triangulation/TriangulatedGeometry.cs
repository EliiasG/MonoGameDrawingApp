using System.Linq;

namespace MonoGameDrawingApp.VectorSprites.Export.Triangulation
{
    public class TriangulatedGeometry
    {
        public TriangulatedGeometry(IPolygonTriangulator triangulator, ModifiedGeometry modifiedGeometry)
        {
            Polygons = (from Polygon polygon in modifiedGeometry.ModifiedPolygons
                        where polygon.Vertices.Length >= 3
                        select triangulator.Triangulate(polygon)).ToArray();
        }

        public TriangulatedPolygon[] Polygons { get; init; }

    }
}
