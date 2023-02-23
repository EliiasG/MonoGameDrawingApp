using Microsoft.Xna.Framework;
using MonoGameDrawingApp.VectorSprites.Export.Triangulation;

namespace MonoGameDrawingApp.VectorSprites.Rendering.MonoGame
{
    public class MonoGameTriangulatedGeometryRenderer
    {
        private readonly MonoGameTriangulatedPolygonRenderer _polygonRenderer = new();

        public void Render(TriangleBatch triangleBatch, TriangulatedGeometry triangulatedGeometry, Vector2 position)
        {
            foreach (TriangulatedPolygon triangulatedPolygon in triangulatedGeometry.Polygons)
            {
                _polygonRenderer.Render(triangleBatch, triangulatedPolygon, position);
            }
        }
    }
}
