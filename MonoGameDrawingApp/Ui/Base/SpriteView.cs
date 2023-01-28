using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameDrawingApp.Ui.Base;
using System.Collections.Generic;

namespace MonoGameDrawingApp.Ui
{
    public class SpriteView : IUiElement
    {
        public readonly string Path;

        private readonly UiEnvironment _environment;

        private Texture2D _texture = null;

        private readonly RenderHelper _renderHelper;

        private static Dictionary<string, Texture2D> _sprites = null;
        private bool _changed = true;

        public SpriteView(UiEnvironment environment, string path)
        {
            _environment = environment;
            Path = path;
            _renderHelper = new RenderHelper();
            if (_sprites == null)
            {
                _sprites = new Dictionary<string, Texture2D>();
            }

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
                if (!_sprites.ContainsKey(Path))
                {
                    _sprites.Add(Path, graphics.Content.Load<Texture2D>(Path));
                }
                _texture = _sprites[Path];
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
