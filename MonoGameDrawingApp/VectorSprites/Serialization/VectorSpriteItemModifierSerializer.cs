using MonoGameDrawingApp.VectorSprites.Modifiers;
using MonoGameDrawingApp.VectorSprites.Modifiers.Applyable.Simple;
using MonoGameDrawingApp.VectorSprites.Serialization.Modifiers;
using System;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace MonoGameDrawingApp.VectorSprites.Serialization
{
    public static class VectorSpriteItemModifierSerializer
    {
        public static ISerializableVectorSpriteItemModifier Serialize(IVectorSpriteItemModifier modifier)
        {
            return modifier switch
            {
                MoveModifier m => new SerializableMoveModifier(m),

                _ => throw new NotImplementedException("No definition for serializing '" + modifier.GetType().Name + "'"),
            };
        }

        public static ISerializableVectorSpriteItemModifier FromJsonNode(JsonNode jsonObject)
        {
            return (jsonObject["Type"]?.ToString()) switch
            {
                null => throw new ArgumentException("Invalid modifier, does not contain type \n \"" + jsonObject.ToString() + "\""),

                "Move" => jsonObject.Deserialize<SerializableMoveModifier>(),

                _ => throw new NotImplementedException("Could not find type + '" + jsonObject["Type"]?.ToString() + "'"),
            };
        }

        public static JsonNode ToJson(ISerializableVectorSpriteItemModifier modifier)
        {
            return JsonSerializer.SerializeToNode(modifier, modifier.GetType());
        }
    }
}
