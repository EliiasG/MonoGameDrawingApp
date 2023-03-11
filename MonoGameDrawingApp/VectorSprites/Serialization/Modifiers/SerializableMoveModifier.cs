using MonoGameDrawingApp.VectorSprites.Modifiers;
using MonoGameDrawingApp.VectorSprites.Modifiers.Applyable.Simple;

namespace MonoGameDrawingApp.VectorSprites.Serialization.Modifiers
{
    public class SerializableMoveModifier : ISerializableVectorSpriteItemModifier
    {
        public SerializableMoveModifier(MoveModifier modifier)
        {
            Offset = new SerializablleVector2(modifier.Offset);
        }

        public SerializableMoveModifier() 
        {
        }

        public string Type 
        { 
            get => "Move";
            set { } //For serialization
        }

        public SerializablleVector2 Offset { get; set; }

        public IVectorSpriteItemModifier ToModifier()
        {
            return new MoveModifier()
            {
                Offset = Offset.ToVector()
            };
        }
    }
}
