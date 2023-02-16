namespace MonoGameDrawingApp.VectorSprites.Serialization
{
    public class SerializableVectorSprite
    {
        public SerializableVectorSprite() { }

        public SerializableVectorSprite(VectorSprite sprite)
        {
            Root = new SerializableVectorSpriteItem(sprite.Root);
        }

        public SerializableVectorSpriteItem Root { get; set; }

        public VectorSprite ToSprite()
        {
            VectorSprite res = new();
            res.Root = Root.ToSpriteItem(res);
            return res;
        }
    }
}
