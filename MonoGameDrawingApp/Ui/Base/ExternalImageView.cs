using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameDrawingApp.Ui.Base
{
    public class ExternalImageView : IUiElement
    {
        public readonly string Path;

        private readonly UiEnvironment _environment;

        private Texture2D _texture = null;

        private readonly RenderHelper _renderHelper;

        private bool _changed = true;

        public ExternalImageView(UiEnvironment environment, string path)
        {
            _environment = environment;
            Path = path;
            _renderHelper = new RenderHelper();
        }

        public int RequiredWidth => _texture == null ? 1 : _texture.Width;

        public int RequiredHeight => _texture == null ? 1 : _texture.Height;

        public bool Changed => _changed;

        public UiEnvironment Environment => _environment;

        public Texture2D Render(Graphics graphics, int width, int height)
        {
            _changed = false;
            _renderHelper.Begin(graphics, width, height);

            if (_texture == null)
            {
                try
                {
                    _texture = Texture2D.FromFile(graphics.Device, Path);
                }
                catch
                {
                    _texture = new ColorRect(Environment, Color.Transparent).Render(graphics, 1, 1);
                }
                _changed = true;
            }

            if (_renderHelper.SizeChanged)
            {
                _renderHelper.BeginSpriteBatchDraw();

                graphics.SpriteBatch.Draw(
                    texture: _texture,
                    position: Vector2.Zero,
                    color: Color.White
                );

                _renderHelper.FinishSpriteBatchDraw();
            }

            return _renderHelper.Result;
        }

        public void Update(Vector2 position, int width, int height)
        {
        }
    }
}
