using Microsoft.Xna.Framework.Graphics;

namespace MonoGameDrawingApp
{
    public class Graphics
    {
        public readonly GraphicsDevice Device;
        public readonly SpriteBatch SpriteBatch;
        public readonly TriangleBatch TriangleBatch;

        public Graphics(GraphicsDevice device, SpriteBatch spriteBatch, TriangleBatch triangleBatch)
        {
            Device = device;
            SpriteBatch = spriteBatch;
            TriangleBatch = triangleBatch;
        }
    }
}
