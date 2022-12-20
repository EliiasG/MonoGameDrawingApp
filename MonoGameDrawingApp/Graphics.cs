using Microsoft.Xna.Framework.Graphics;
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

        public Graphics(GraphicsDevice device, SpriteBatch spriteBatch)
        {
            Device = device;
            SpriteBatch = spriteBatch;
        }
    }
}
