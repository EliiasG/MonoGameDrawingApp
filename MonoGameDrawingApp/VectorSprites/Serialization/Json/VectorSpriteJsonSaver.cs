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
            CurrentlySaving = true;
            Task.Run(() =>
            {
                Save(path);
                CurrentlySaving = false;
            });
        }

        public void Save(string path)
        {
            JsonSerializerOptions options = new()
            {
                WriteIndented = true,
                MaxDepth = int.MaxValue,
            };

            SerializableVectorSprite serializableVectorSprite = new(Sprite);
            using FileStream createStream = File.Create(path);
            JsonSerializer.Serialize(createStream, serializableVectorSprite, options);
        }
    }
}
