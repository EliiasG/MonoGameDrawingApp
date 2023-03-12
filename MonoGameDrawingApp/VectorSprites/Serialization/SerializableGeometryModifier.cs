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
                (IGeometryModifierParameter p) => _convertValue(p.ObjectValue)
            ).ToArray();
        }

        public string Name { get; set; }

        public object[] Parameters { get; set; }

        public IGeometryModifier ToModifier()
        {
            IGeometryModifier modifier = GeometryModifierRegistry.GenerateFromName(Name);

            int i = 0;
            foreach (IGeometryModifierParameter parameter in modifier.Parameters)
            {
                _setParameter(parameter, Parameters[i++]);
            }

            return modifier;
        }

        private void _setParameter(IGeometryModifierParameter parameter, object value)
        {
            JsonElement? jsonElement = value as JsonElement?;
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
                default:
                    throw new NotImplementedException($"Could not convert to type '{parameter.ObjectValue.GetType().Name}' :\n{jsonElement.ToString()}");
            }
        }

        private object _convertValue(object value)
        {
            return value switch
            {
                Color v => new SerializableColor(v),
                Vector2 v => new SerializablleVector2(v),
                _ => value,
            };
        }

    }
}
