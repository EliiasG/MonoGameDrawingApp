namespace MonoGameDrawingApp.VectorSprites.Modifiers.Applyable
{
    public interface IApplyableVectorSpriteItemModifier : IVectorSpriteItemModifier
    {
        void Apply(VectorSpriteItem Item);
    }
}
