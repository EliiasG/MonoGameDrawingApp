using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoGameDrawingApp
{
    public class Graphics
    {
        public readonly GraphicsDevice Device;
        public readonly SpriteBatch SpriteBatch;
        public MouseCursor Cursor;

        public Graphics(GraphicsDevice device, SpriteBatch spriteBatch)
        {
            Device = device;
            SpriteBatch = spriteBatch;
            Cursor = MouseCursor.Arrow;
        }
    }
}
