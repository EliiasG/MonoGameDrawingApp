using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameDrawingApp.Ui.Base
{
    public class ColorRect : IUiElement
    {
        private Color _color;
        private readonly RenderHelper _renderHelper;

        public ColorRect(UiEnvironment environment, Color color)
        {
            Environment = environment;

            Color = color;
            _renderHelper = new RenderHelper();
        }

        public Color Color
        {
            get => _color;
            set
            {
                if (_color != value)
                {
                    _color = value;
                    Changed = true;
                }
            }
        }

        public int RequiredWidth => 1;

        public int RequiredHeight => 1;

        public bool Changed { get; private set; }

        public UiEnvironment Environment { get; }

        public Texture2D Render(Graphics graphics, int width, int height)
        {
            _renderHelper.Begin(graphics, width, height);

            if (_renderHelper.SizeChanged || Changed)
            {
                _renderHelper.BeginDraw();

                graphics.Device.Clear(Color);

                _renderHelper.FinishDraw();
            }

            Changed = false;
            return _renderHelper.Result;
        }
        public void Update(Vector2 position, int width, int height) { }
    }
}
