using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace MonoGameDrawingApp.Ui.Base
{
    public class SpriteView : IUiElement
    {
        private Texture2D _texture;

        private readonly RenderHelper _renderHelper;

        private static Dictionary<string, Texture2D> _sprites;

        public SpriteView(UiEnvironment environment, string path)
        {
            Environment = environment;
            Path = path;
            _renderHelper = new RenderHelper();
            _sprites ??= new Dictionary<string, Texture2D>();

        }
        public int RequiredWidth => _texture == null ? 1 : _texture.Width;

        public int RequiredHeight => _texture == null ? 1 : _texture.Height;

        public bool Changed { get; private set; } = true;

        public UiEnvironment Environment { get; }

        public string Path { get; }

        public Texture2D Render(Graphics graphics, int width, int height)
        {
            Changed = false;
            _renderHelper.Begin(graphics, width, height);

            if (_texture == null)
            {
                if (!_sprites.ContainsKey(Path))
                {
                    _sprites.Add(Path, Environment.Content.Load<Texture2D>(Path));
                }
                _texture = _sprites[Path];
                Changed = true;
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
