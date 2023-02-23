namespace MonoGameDrawingApp.VectorSprites.Export.Triangulation
{
    public interface IPolygonTriangulator
    {
        public TriangulatedPolygon Triangulate(Polygon polygon);
    }
}
