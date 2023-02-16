using MonoGameDrawingApp.Ui.FileSystemTrees;
using MonoGameDrawingApp.VectorSprites;
using MonoGameDrawingApp.VectorSprites.Serialization.Json;

namespace MonoGameDrawingApp.Ui.DrawingApp.Tabs.VectorSprites
{
    public class VectorSpriteFileCreator : IFileCreator
    {
        public void CreateFile(string path)
        {
            VectorSprite sprite = new();
            sprite.Root = new VectorSpriteItem("Root", sprite);

            VectorSpriteJsonSaver saver = new(sprite);
            saver.StartSaving(path);
            while (saver.CurrentlySaving) ;
        }
    }
}
