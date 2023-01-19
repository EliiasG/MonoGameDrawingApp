using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameDrawingApp.Ui
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
                if (_text != value)
                {
                    _text = value;
                    _changed = true;
                }
            }
        }

        public Texture2D Render(Graphics graphics, int width, int height)
        {
            _renderHelper.Begin(graphics, width, height);

            if (Changed || _renderHelper.SizeChanged)
            {
                _renderHelper.BeginDraw();

                graphics.SpriteBatch.DrawString(
                    spriteFont: Environment.Font,
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
