namespace MonoGameDrawingApp
{
    public class Util
    {
        public static float Distance(Microsoft.Xna.Framework.Vector2 a, Microsoft.Xna.Framework.Vector2 b)
        {
            return (a - b).Length();
        }

        public static float DistanceSquared(Microsoft.Xna.Framework.Vector2 a, Microsoft.Xna.Framework.Vector2 b)
        {
            return (a - b).LengthSquared();
        }

        public static Microsoft.Xna.Framework.Color InvertColor(Microsoft.Xna.Framework.Color color)
        {
            return new Microsoft.Xna.Framework.Color(
                r: 255 - color.R,
                g: 255 - color.G,
                b: 255 - color.B,
                alpha: color.A
            );
        }

        public static System.Numerics.Vector2 ToNumericsVector2(Microsoft.Xna.Framework.Vector2 vector)
        {
            return new System.Numerics.Vector2(vector.X, vector.Y);
        }

        public static Microsoft.Xna.Framework.Color ToXnaColor(System.Drawing.Color color)
        {
            return new Microsoft.Xna.Framework.Color(color.R, color.B, color.G, color.A);
        }

        public static System.Drawing.Color ToDrawingColor(Microsoft.Xna.Framework.Color color)
        {
            return System.Drawing.Color.FromArgb(color.A, color.R, color.B, color.G);
        }
    }
}
