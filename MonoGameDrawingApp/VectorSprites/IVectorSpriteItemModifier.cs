using MonoGameDrawingApp.VectorSprites.Export;

namespace MonoGameDrawingApp.VectorSprites
{
    public interface IVectorSpriteItemModifier
    {
        void Apply(ModifiedGeometry geometry);
    }
}
