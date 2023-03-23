using MonoGameDrawingApp.VectorSprites;
using MonoGameDrawingApp.VectorSprites.Attachments;
using MonoGameDrawingApp.VectorSprites.Export;
using MonoGameDrawingApp.VectorSprites.Export.Triangulation;

namespace MonoGameDrawingApp.Ui.DrawingApp.Tabs.VectorSprites.Preview
{
    public class PreviewSpriteItemAttachment : IVectorSpriteItemAttachment
    {
        public PreviewSpriteItemAttachment(VectorSpriteItem item, IPolygonTriangulator triangulator)
        {
            Triangulator = triangulator;
            Item = item;
            _retriangulate();
        }

        public TriangulatedGeometry TriangulatedGeometry { get; private set; }

        public VectorSpriteItem Item { get; init; }

        public IPolygonTriangulator Triangulator { get; init; }

        public void DataChanged()
        {
            _retriangulate();
        }

        private void _retriangulate()
        {
            TriangulatedGeometry = new TriangulatedGeometry(Triangulator, new ModifiedGeometry(Item.Geometry));
        }
    }
}
