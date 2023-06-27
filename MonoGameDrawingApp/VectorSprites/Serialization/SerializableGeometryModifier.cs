using MonoGameDrawingApp.VectorSprites.Modifiers;
using MonoGameDrawingApp.VectorSprites.Modifiers.Parameters;
using System;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text.Json;

namespace MonoGameDrawingApp.VectorSprites.Serialization
{
    public class SerializableGeometryModifier
    {
        public SerializableGeometryModifier()
        {
        }
        public SerializableGeometryModifier(IGeometryModifier modifier)
        {
            Name = modifier.Name;
            Parameters = modifier.Parameters.Select(
                (IGeometryModifierParameter p) => ConvertValue(p.ObjectValue)
            ).ToArray();
        }

        public string Name { get; set; }

        public object[] Parameters { get; set; }

        public IGeometryModifier ToModifier(VectorSprite sprite)
        {
            IGeometryModifier modifier = GeometryModifierRegistry.GenerateFromName(Name);

            int i = 0;
            foreach (IGeometryModifierParameter parameter in modifier.Parameters)
            {
                SetParameter(parameter, Parameters[i++], sprite);
            }

            return modifier;
        }

        private static void SetParameter(IGeometryModifierParameter parameter, object value, VectorSprite sprite)
        {
            JsonElement? jsonElement = value as JsonElement?;
            //ide is dumb, don't use the expression here
            switch (parameter)
            {
                case GeometryModifierParameter<Vector2> v:
                    v.Value = jsonElement?.Deserialize<SerializablleVector2>().ToVector() ?? ((SerializablleVector2)value).ToVector();
                    break;
                case GeometryModifierParameter<int> v:
                    v.Value = jsonElement?.Deserialize<int>() ?? (int)value;
                    break;
                case GeometryModifierParameter<float> v:
                    v.Value = jsonElement?.Deserialize<float>() ?? (float)value;
                    break;
                case GeometryModifierParameter<bool> v:
                    v.Value = jsonElement?.Deserialize<bool>() ?? (bool)value;
                    break;
                case GeometryModifierParameter<Color> v:
                    v.Value = jsonElement?.Deserialize<SerializableColor>().ToColor() ?? ((SerializableColor)value).ToColor();
                    break;
                case GeometryModifierParameter<VectorSpriteItemReference> v:
                    v.Value = jsonElement?.Deserialize<SerializableVectorSpriteItemReference>().ToReference(sprite) ?? ((SerializableVectorSpriteItemReference)value).ToReference(sprite);
                    break;
                default:
                    throw new NotImplementedException($"Could not convert to type '{parameter.ObjectValue.GetType().Name}' :\n{jsonElement}");
            }
        }

        private static object ConvertValue(object value)
        {
            return value switch
            {
                Color v => new SerializableColor(v),
                Vector2 v => new SerializablleVector2(v),
                VectorSpriteItemReference v => new SerializableVectorSpriteItemReference(v),
                _ => value,
            };
        }

    }
}
