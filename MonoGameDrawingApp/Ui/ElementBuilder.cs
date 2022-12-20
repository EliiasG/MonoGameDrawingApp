using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameDrawingApp.Ui
{
    public class ElementBuilder
    {
        public readonly Graphics Graphics;
        public readonly RenderTarget2D RenderTarget;

        public ElementBuilder(Graphics graphics, int width, int height)
        {
            Graphics = graphics;
            RenderTarget = new RenderTarget2D(Graphics.Device, width, height);
            graphics.Device.SetRenderTarget(RenderTarget);
            graphics.Device.Clear(Color.Transparent);
            graphics.SpriteBatch.Begin();
        }

        public Texture2D Finish()
        {
            Graphics.SpriteBatch.End();
            Graphics.Device.SetRenderTarget(null);
            return RenderTarget;
        }
    }
}
