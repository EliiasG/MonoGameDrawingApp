using MonoGameDrawingApp.VectorSprites.Export;
using MonoGameDrawingApp.VectorSprites.Modifiers.Parameters;
using System.Collections.Generic;

namespace MonoGameDrawingApp.VectorSprites.Modifiers
{
    public interface IGeometryModifier
    {
        string Name { get; }

        IEnumerable<IGeometryModifierParameter> Parameters { get; }

        void Modify(ModifiedGeometry geometry);
    }
}
