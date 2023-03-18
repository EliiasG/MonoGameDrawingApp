namespace MonoGameDrawingApp.VectorSprites.Serialization
{
    public class SerializableVectorSpriteItemReference
    {
        public SerializableVectorSpriteItemReference() { }

        public SerializableVectorSpriteItemReference(VectorSpriteItemReference reference)
        {
            Path = reference.Path;
        }

        public string Path { get; set; }

        public VectorSpriteItemReference ToReference(VectorSprite sprite)
        {
            return new VectorSpriteItemReference(Path, sprite);
        }
    }
}
