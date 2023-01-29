using Microsoft.Xna.Framework.Graphics;

namespace MonoGameDrawingApp
{
    public class Graphics
    {
        public readonly GraphicsDevice Device;
        public readonly SpriteBatch SpriteBatch;

        public Graphics(GraphicsDevice device, SpriteBatch spriteBatch)
        {
            Device = device;
            SpriteBatch = spriteBatch;
        }
    }
}
