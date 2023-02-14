using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace MonoGameDrawingApp.VectorSprites.Serialization.Json
{
    public class VectorSpriteItemJsonSaver
    {
        public VectorSpriteItemJsonSaver(VectorSprite sprite)
        {
            Sprite = sprite;
            CurrentlySaving = false;
        }

        public bool CurrentlySaving { get; private set; }

        public VectorSprite Sprite { get; init; }

        public void StartSaving(string path)
        {
            CurrentlySaving = true;
            SerializableVectorSprite serializableVectorSprite = new(Sprite);
            Task.Run(async () =>
            {
                using FileStream createStream = File.Create(path);
                await JsonSerializer.SerializeAsync(createStream, serializableVectorSprite);
                await createStream.DisposeAsync();
                CurrentlySaving = false;
            });
        }
    }
}
