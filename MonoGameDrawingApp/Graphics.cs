using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
