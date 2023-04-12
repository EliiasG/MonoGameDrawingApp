namespace MonoGameDrawingApp.VectorSprites.Export.Triangulation
{
    public interface IPolygonTriangulator
    {
        public TriangulatedPolygon Triangulate(Polygon polygon);

        public static IPolygonTriangulator CreateDefault()
        {
            return new EarClipperLibGeometryTriangulator();
        }
    }
}
