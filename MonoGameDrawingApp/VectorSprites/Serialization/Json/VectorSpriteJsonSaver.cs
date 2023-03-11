using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace MonoGameDrawingApp.VectorSprites.Serialization.Json
{
    public class VectorSpriteJsonSaver
    {
        public VectorSpriteJsonSaver(VectorSprite sprite)
        {
            Sprite = sprite;
            CurrentlySaving = false;
        }

        public bool CurrentlySaving { get; private set; }

        public VectorSprite Sprite { get; init; }

        public void StartSaving(string path)
        {
            JsonSerializerOptions options = new()
            {
                WriteIndented = true,
                MaxDepth = int.MaxValue,
            };

            options.Converters.Add(new ModifierJsonConverter());

            CurrentlySaving = true;
            SerializableVectorSprite serializableVectorSprite = new(Sprite);
            Task.Run(async () =>
            {
                using FileStream createStream = File.Create(path);
                await JsonSerializer.SerializeAsync(createStream, serializableVectorSprite, options);
                await createStream.DisposeAsync();
                CurrentlySaving = false;
            });
        }
    }
}
