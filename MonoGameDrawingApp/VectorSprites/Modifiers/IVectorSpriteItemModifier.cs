using MonoGameDrawingApp.VectorSprites.Export;
using System;

namespace MonoGameDrawingApp.VectorSprites.Modifiers
{
    public interface IVectorSpriteItemModifier
    {
        void Modify(ModifiedGeometry geometry);

        Action Changed { get; set; }

        Action Changing { get; set; }
    }
}
