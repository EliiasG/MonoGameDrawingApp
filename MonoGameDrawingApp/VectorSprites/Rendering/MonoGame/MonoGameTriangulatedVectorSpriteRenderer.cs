using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameDrawingApp.VectorSprites.Export.Triangulation;
using System.Collections.Generic;
using System.Linq;

namespace MonoGameDrawingApp.VectorSprites.Rendering.MonoGame
{
    public static class MonoGameTriangulatedVectorSpriteRenderer
    {
        public static void Render(TriangleBatch triangleBatch, TriangulatedVectorSprite triangulatedSprite, Vector2 position)
        {
            List<VertexPositionColor> vertexPositionColors = new();

            for (int i = 0; i < triangulatedSprite.Vertices.Length; i++)
            {
                Vector2 vertex = triangulatedSprite.Vertices[i] + position;
                vertexPositionColors.Add(new VertexPositionColor(new Vector3(vertex.X, vertex.Y, 0f), Util.ToXnaColor(triangulatedSprite.Colors[i])));
            }

            triangleBatch.DrawTriangles(vertexPositionColors.ToArray(), triangulatedSprite.Indices.ToArray());
        }
    }
}
