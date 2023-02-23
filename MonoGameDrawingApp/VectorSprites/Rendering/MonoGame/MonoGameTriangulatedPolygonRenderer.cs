using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameDrawingApp.VectorSprites.Export.Triangulation;
using System.Linq;


namespace MonoGameDrawingApp.VectorSprites.Rendering.MonoGame
{
    public class MonoGameTriangulatedPolygonRenderer
    {
        public void Render(TriangleBatch triangleBatch, TriangulatedPolygon triangulatedPolygon, Vector2 position)
        {
            VertexPositionColor[] vertexPositionColors = (
                from System.Numerics.Vector2 v in triangulatedPolygon.Vertices
                select new VertexPositionColor(new Vector3(v.X + position.X, v.Y + position.Y, 0f), Util.ToXnaColor(triangulatedPolygon.Color))
            ).ToArray();

            triangleBatch.DrawTriangles(vertexPositionColors, triangulatedPolygon.Indices);
        }
    }
}
