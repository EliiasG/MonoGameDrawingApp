using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameDrawingApp.Ui
{
    public class ColorRect : IUiElement
    {
        public Color Color;

        public ColorRect(Color color)
        {
            Color = color;
        }

        public int RequiredWidth => 1;

        public int RequiredHeight => 1;

        public Texture2D Render(Graphics graphics, Vector2 position, int width, int height)
        {
            RenderTarget2D renderTarget = new RenderTarget2D(graphics.Device, width, height);
            graphics.Device.SetRenderTarget(renderTarget);
            graphics.Device.Clear(Color);
            graphics.Device.SetRenderTarget(null);
            return renderTarget;
        }
    }
}
