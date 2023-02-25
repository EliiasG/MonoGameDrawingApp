namespace MonoGameDrawingApp
{
    public class Util
    {
        public static System.Numerics.Vector2 ToNumericsVector2(Microsoft.Xna.Framework.Vector2 vector)
        {
            return new System.Numerics.Vector2(vector.X, vector.Y);
        }

        public static Microsoft.Xna.Framework.Color ToXnaColor(System.Drawing.Color color)
        {
            return new Microsoft.Xna.Framework.Color(color.R, color.B, color.G, color.A);
        }
    }
}
