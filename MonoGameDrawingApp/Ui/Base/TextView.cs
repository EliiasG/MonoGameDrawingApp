using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace MonoGameDrawingApp.Ui.Base
{
    public class TextView : IUiElement
    {

        private readonly UiEnvironment _environment;

        private string _text;
        private bool _changed = true;

        private RenderHelper _renderHelper;

        public TextView(UiEnvironment environment, string text)
        {
            _environment = environment;

            _renderHelper = new RenderHelper();
            Text = text;
        }

        public int RequiredWidth => (int)Environment.Font.MeasureString(Text).X;

        public int RequiredHeight => (int)Environment.Font.MeasureString(Text).Y;

        public bool Changed => _changed;

        public UiEnvironment Environment => _environment;

        public string Text
        {
            get => _text;
            set
            {
                ICollection<char> chars = Environment.Font.Characters;
                if (_text != value)
                {
                    _text = string.Concat(value.Select((c) => chars.Contains(c) ? c : '?'));
                    _changed = true;
                }
            }
        }

        public Texture2D Render(Graphics graphics, int width, int height)
        {
            _renderHelper.Begin(graphics, width, height);

            if (Changed || _renderHelper.SizeChanged)
            {
                _renderHelper.BeginSpriteBatchDraw();

                graphics.SpriteBatch.DrawString(
                    spriteFont: Environment.Font,
                    text: Text,
                    position: Vector2.Zero,
                    color: Color.White
                );

                _renderHelper.FinishSpriteBatchDraw();
            }

            _changed = false;
            return _renderHelper.Result;
        }

        public void Update(Vector2 position, int width, int height)
        {
        }
    }
}
