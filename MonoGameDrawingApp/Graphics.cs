using Microsoft.Xna.Framework.Graphics;

namespace MonoGameDrawingApp
{
    public class Graphics
    {
        public GraphicsDevice Device { get; }

        public SpriteBatch SpriteBatch { get; }

        public TriangleBatch TriangleBatch { get; }

        public Graphics(GraphicsDevice device, SpriteBatch spriteBatch, TriangleBatch triangleBatch)
        {
            Device = device;
            SpriteBatch = spriteBatch;
            TriangleBatch = triangleBatch;
        }
    }
}
