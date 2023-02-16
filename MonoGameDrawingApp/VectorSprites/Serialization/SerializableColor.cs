using System.Drawing;

namespace MonoGameDrawingApp.VectorSprites.Serialization
{
    public class SerializableColor
    {
        public SerializableColor() { }

        public SerializableColor(Color color)
        {
            R = color.R;
            G = color.G;
            B = color.B;
            A = color.A;
        }

        public byte R { get; set; }
        public byte G { get; set; }
        public byte B { get; set; }
        public byte A { get; set; }

        public Color ToColor()
        {
            return Color.FromArgb(A, R, G, B);
        }
    }
}
