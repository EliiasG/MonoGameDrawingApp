using MonoGameDrawingApp.VectorSprites;
using MonoGameDrawingApp.VectorSprites.Attachments.ChangeListener;
using MonoGameDrawingApp.VectorSprites.Export.Triangulation;
using MonoGameDrawingApp.VectorSprites.Serialization.Json;

namespace MonoGameDrawingApp.Export.VectorSprites
{
    public abstract class VectorSpriteExporter : IFileExporter
    {
        public string InputExtention => "vecspr";

        public abstract string OutputExtention { get; }

        public void Export(string fromPath, string exportFilePath)
        {
            VectorSprite vectorSprite = VectorSpriteJsonLoader.LoadVectorSprite(fromPath);
            vectorSprite.AddAttachment(new ChangeListenerVectorSpriteAttachment(() => { }));
            ExportSprite(new TriangulatedVectorSprite(vectorSprite), exportFilePath);
        }

        protected abstract void ExportSprite(TriangulatedVectorSprite sprite, string exportFilePath);
    }
}
