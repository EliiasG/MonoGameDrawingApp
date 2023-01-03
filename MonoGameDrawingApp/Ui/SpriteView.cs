using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameDrawingApp.Ui
{
    public class SpriteView : IUiElement
    {
        public readonly string Path;
        public Color Color;

        private Texture2D _texture = null;
        private readonly RenderHelper _renderHelper;

        private static Dictionary<string, Texture2D> _sprites = null;

        public SpriteView(string path, Color color = default)
        {
            Path = path;
            Color = color;
            _renderHelper = new RenderHelper();
            if(_sprites == null)
            {
                _sprites = new Dictionary<string, Texture2D>();
            }
        }

        public int RequiredWidth => _texture == null ? 1 : _texture.Width;

        public int RequiredHeight => _texture == null ? 1 : _texture.Height;

        public Texture2D Render(Graphics graphics, Vector2 position, int width, int height)
        {
            if (_texture == null)
            {
                if (!_sprites.ContainsKey(Path))
                {
                    _sprites.Add(Path, graphics.Content.Load<Texture2D>(Path));
                }
                _texture = _sprites[Path];
            }

            _renderHelper.Begin(graphics, width, height);

            graphics.SpriteBatch.Draw(
                texture: _texture,
                position: Vector2.Zero,
                color: Color
            );

            return _renderHelper.FinishDraw();
        }
    }
}
