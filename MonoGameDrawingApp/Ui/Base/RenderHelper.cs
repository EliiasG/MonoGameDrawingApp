using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Threading.Tasks;

namespace MonoGameDrawingApp.Ui.Base
{
    public class RenderHelper
    {
        private RenderTarget2D _renderTarget;
        private Graphics _graphics;

        private int _width;
        private int _height;

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

            _renderTarget ??= new RenderTarget2D(graphics.Device, Math.Max(width, 1), Math.Max(height, 1));
        }

        public void BeginDraw()
        {
            _graphics.Device.SetRenderTarget(_renderTarget);
            _graphics.Device.Clear(Color.Transparent);
        }

        public void BeginSpriteBatchDraw()
        {
            BeginDraw();
            _graphics.SpriteBatch.Begin();
        }

        public void FinishDraw()
        {
            _changed = false;
            _graphics.Device.SetRenderTarget(null);
        }

        public void FinishSpriteBatchDraw()
        {
            _graphics.SpriteBatch.End();
            FinishDraw();
        }

        ~RenderHelper()
        {
            Task.Run(() => _renderTarget?.Dispose());
        }
    }
}
