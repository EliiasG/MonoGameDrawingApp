using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameDrawingApp.Ui
{
    public class TextView : IUiElement
    {
        public readonly SpriteFont Font;
        public string Text 
        {
            get => _text;
            set
            {
                _text = value;
                _changed = true;
            }
        }

        private string _text;
        private bool _changed = true;

        private RenderHelper _renderHelper;

        public TextView(SpriteFont font, string text, Color color = new Color())
        {
            Font = font;
            _renderHelper = new RenderHelper();
            Text = text;
        }

        public int RequiredWidth => (int)Font.MeasureString(Text).X;

        public int RequiredHeight => (int)Font.MeasureString(Text).Y;

        public bool Changed => _changed;

        public Texture2D Render(Graphics graphics, int width, int height)
        {
            _renderHelper.Begin(graphics, width, height);

            if (Changed || _renderHelper.SizeChanged)
            {
                _renderHelper.BeginDraw();

                graphics.SpriteBatch.DrawString(
                    spriteFont: Font,
                    text: Text,
                    position: Vector2.Zero,
                    color: Color.White
                );

                _renderHelper.FinishDraw();
            }

            _changed = false;
            return _renderHelper.Result;
        }

        public void Update(Vector2 position, int width, int height)
        {
        }
    }
}
