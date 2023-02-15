using System.Numerics;

namespace MonoGameDrawingApp.VectorSprites.Serialization
{
    public class SerializablleVector2
    {
        public SerializablleVector2(Vector2 vector)
        {
            X = vector.X;
            Y = vector.Y;
        }

        public float X { get; set; }
        public float Y { get; set; }

        public Vector2 ToVector()
        {
            return new Vector2(X, Y);
        }
    }
}
