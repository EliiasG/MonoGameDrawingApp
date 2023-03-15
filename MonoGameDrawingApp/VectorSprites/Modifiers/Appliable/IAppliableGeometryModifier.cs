namespace MonoGameDrawingApp.VectorSprites.Modifiers.Appliable
{
    public interface IAppliableGeometryModifier : IGeometryModifier
    {
        void Apply(VectorSpriteItem item);
    }
}
