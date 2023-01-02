using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoGameDrawingApp
{
    public class Graphics
    {
        public readonly GraphicsDevice Device;
        public readonly SpriteBatch SpriteBatch;
        public MouseCursor Cursor;
        public ContentManager Content;

        public Graphics(GraphicsDevice device, SpriteBatch spriteBatch, ContentManager content)
        {
            Device = device;
            SpriteBatch = spriteBatch;
            Cursor = MouseCursor.Arrow;
            Content = content;
        }
    }
}
