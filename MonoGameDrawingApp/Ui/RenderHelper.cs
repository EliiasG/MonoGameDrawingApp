using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameDrawingApp.Ui
{
    public class RenderHelper
    {
        private RenderTarget2D _renderTarget = null;
        private Graphics _graphics = null;

        public void Begin(Graphics graphics, int width, int height)
        {
            _graphics = graphics;

            if (_renderTarget != null && (_renderTarget.Width != width || _renderTarget.Height != height))
            {
                _renderTarget.Dispose();
                _renderTarget = null;
            }

            if (_renderTarget == null)
            {
                _renderTarget = new RenderTarget2D(graphics.Device, width, height);
            }

            graphics.Device.SetRenderTarget(_renderTarget);
            graphics.Device.Clear(Color.Transparent);
            graphics.SpriteBatch.Begin();
        }

        public Texture2D Finish()
        {
            _graphics.SpriteBatch.End();
            _graphics.Device.SetRenderTarget(null);
            return _renderTarget;
        }
    }
}
