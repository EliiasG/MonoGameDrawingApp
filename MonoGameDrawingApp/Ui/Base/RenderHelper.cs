using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Threading.Tasks;

namespace MonoGameDrawingApp.Ui.Base
{
    public class RenderHelper
    {
        private RenderTarget2D _renderTarget = null;
        private Graphics _graphics = null;

        private int _width = 0;
        private int _height = 0;

        private bool _changed = true;

        public Texture2D Result => _renderTarget;

        public bool SizeChanged
        {
            get
            {
                _changed = _renderTarget.Width != _width || _renderTarget.Height != _height || _changed;
                return _changed;
            }
        }

        public void Begin(Graphics graphics, int width, int height)
        {
            _graphics = graphics;

            _width = width;
            _height = height;

            if (_renderTarget != null && SizeChanged)
            {
                _renderTarget.Dispose();
                _renderTarget = null;
            }

            if (_renderTarget == null)
            {
                _renderTarget = new RenderTarget2D(graphics.Device, Math.Max(width, 1), Math.Max(height, 1));
            }
        }

        public void BeginDraw()
        {
            _graphics.Device.SetRenderTarget(_renderTarget);
            _graphics.Device.Clear(Color.Transparent);
            _graphics.SpriteBatch.Begin();
        }

        ~RenderHelper()
        {
            Task.Run(() => _renderTarget?.Dispose());
        }

        public void FinishDraw()
        {
            _changed = false;
            _graphics.SpriteBatch.End();
            _graphics.Device.SetRenderTarget(null);
        }

    }
}
