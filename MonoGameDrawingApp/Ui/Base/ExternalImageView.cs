using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

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
                FileStream fileStream = new FileStream(Path, FileMode.Open);
                _texture = Texture2D.FromStream(graphics.Device, fileStream);
                fileStream.Dispose();
                _changed = true;
            }

            if (_renderHelper.SizeChanged)
            {
                _renderHelper.BeginDraw();

                graphics.SpriteBatch.Draw(
                    texture: _texture,
                    position: Vector2.Zero,
                    color: Color.White
                );

                _renderHelper.FinishDraw();
            }

            return _renderHelper.Result;
        }

        public void Update(Vector2 position, int width, int height)
        {
        }
    }
}
