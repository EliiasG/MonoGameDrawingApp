using MonoGameDrawingApp.VectorSprites.Serialization.Modifiers;
using System;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace MonoGameDrawingApp.VectorSprites.Serialization.Json
{
    internal class ModifierJsonConverter : JsonConverter<ISerializableVectorSpriteItemModifier>
    {
        public override ISerializableVectorSpriteItemModifier Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return VectorSpriteItemModifierSerializer.FromJsonNode(JsonNode.Parse(ref reader));
        }

        public override void Write(Utf8JsonWriter writer, ISerializableVectorSpriteItemModifier value, JsonSerializerOptions options)
        {
            VectorSpriteItemModifierSerializer.ToJson(value).WriteTo(writer, options);
        }
    }
}
