using System.IO;
using System.Text.Json;

namespace MonoGameDrawingApp.VectorSprites.Serialization.Json
{
    public class VectorSpriteJsonLoader
    {
        public VectorSprite LoadVectorSprite(string path)
        {

            using FileStream openStream = File.OpenRead(path);

            JsonSerializerOptions options = new() { MaxDepth = int.MaxValue };

            SerializableVectorSprite vectorSprite = JsonSerializer.Deserialize<SerializableVectorSprite>(openStream, options);

            openStream.Close();

            return vectorSprite.ToSprite();
        }
    }
}
