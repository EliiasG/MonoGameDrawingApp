using Microsoft.Xna.Framework;
using MonoGameDrawingApp.VectorSprites;
using MonoGameDrawingApp.VectorSprites.Attachments;
using MonoGameDrawingApp.VectorSprites.Export.Triangulation;
using MonoGameDrawingApp.VectorSprites.Rendering.MonoGame;
using System.Linq;

namespace MonoGameDrawingApp.Ui.DrawingApp.Tabs.VectorSprites.Preview
{
    public class PreviewSpriteAttachment : IVectorSpriteAttachment
    {
        private readonly IPolygonTriangulator _triangulator;

        private readonly MonoGameTriangulatedGeometryRenderer _renderer;

        public PreviewSpriteAttachment(VectorSprite sprite)
        {
            _triangulator = IPolygonTriangulator.CreateDefault();
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
            Draw(triangleBatch, Sprite.Root, Vector2.Zero);
        }

        public void Retriangulate()
        {
            Retriangluate(Sprite.Root);
        }

        private void Retriangluate(VectorSpriteItem item)
        {
            item.GetAttachment<PreviewSpriteItemAttachment>().Retriangulate();
            foreach (VectorSpriteItem child in item.Children)
            {
                Retriangluate(child);
            }
        }

        private void Draw(TriangleBatch triangleBatch, VectorSpriteItem item, Vector2 position)
        {
            position += item.Position;

            _renderer.Render(triangleBatch, item.GetAttachment<PreviewSpriteItemAttachment>().TriangulatedGeometry, position);

            foreach (VectorSpriteItem child in item.Children.Reverse())
            {
                Draw(triangleBatch, child, position);
            }
        }
    }
}
