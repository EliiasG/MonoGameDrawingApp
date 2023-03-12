using System;

namespace MonoGameDrawingApp.VectorSprites.Modifiers.Parameters
{
    public interface IGeometryModifierParameter
    {
        string Name { get; }

        Action Changing { get; set; }

        Action Changed { get; set; }

        public object ObjectValue { get; }
    }
}
