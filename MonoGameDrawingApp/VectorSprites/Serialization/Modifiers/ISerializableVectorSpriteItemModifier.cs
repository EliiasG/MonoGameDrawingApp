using MonoGameDrawingApp.VectorSprites.Modifiers;

namespace MonoGameDrawingApp.VectorSprites.Serialization.Modifiers
{
    public interface ISerializableVectorSpriteItemModifier
    {
        string Type { get; }

        public IVectorSpriteItemModifier ToModifier();
    }
}
