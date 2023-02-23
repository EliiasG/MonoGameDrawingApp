namespace MonoGameDrawingApp
{
    public class Util
    {
        public static Microsoft.Xna.Framework.Vector2 ToXnaVector2(System.Numerics.Vector2 vector)
        {
            return new Microsoft.Xna.Framework.Vector2(vector.X, vector.Y);
        }

        public static Microsoft.Xna.Framework.Color ToXnaColor(System.Drawing.Color color)
        {
            return new Microsoft.Xna.Framework.Color(color.R, color.B, color.G, color.A);
        }
    }
}
