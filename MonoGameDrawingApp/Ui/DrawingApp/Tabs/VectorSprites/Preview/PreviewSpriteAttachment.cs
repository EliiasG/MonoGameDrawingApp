using Microsoft.Xna.Framework;
using MonoGameDrawingApp.Ui.DrawingApp.Tabs.VectorSprites.Preview;
using MonoGameDrawingApp.VectorSprites;
using MonoGameDrawingApp.VectorSprites.Attachments;
using MonoGameDrawingApp.VectorSprites.Export.Triangulation;
using MonoGameDrawingApp.VectorSprites.Rendering.MonoGame;
using System.Linq;

namespace MonoGameDrawingApp.Ui.DrawingApp.Tabs.VectorSprites.Rendering
{
    public class PreviewSpriteAttachment : IVectorSpriteAttachment
    {
        private readonly IPolygonTriangulator _triangulator;

        private readonly MonoGameTriangulatedGeometryRenderer _renderer;

        public PreviewSpriteAttachment(VectorSprite sprite)
        {
            _triangulator = new EarClippingGeometryTriangulator();
            _renderer = new MonoGameTriangulatedGeometryRenderer();
            Sprite = sprite;
        }

        public VectorSprite Sprite { get; init; }

        public void Attach(VectorSpriteItem item)
        {
            item.AddAttachment(new PreviewSpriteItemAttachment(item, _triangulator));
        }

        public void Draw(TriangleBatch triangleBatch)
        {
            _draw(triangleBatch, Sprite.Root, Vector2.Zero);
        }

        private void _draw(TriangleBatch triangleBatch, VectorSpriteItem item, Vector2 position)
        {
            position += item.Position;

            _renderer.Render(triangleBatch, item.GetAttachment<PreviewSpriteItemAttachment>().TriangulatedGeometry, position);

            foreach (VectorSpriteItem child in item.Children.Reverse())
            {
                _draw(triangleBatch, child, position);
            }
        }
    }
}
