using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameDrawingApp.Ui.Base
{
    public class ColorRect : IUiElement
    {
        public readonly Color Color;

        private readonly UiEnvironment _environment;

        private RenderHelper _renderHelper;

        public ColorRect(UiEnvironment environment, Color color)
        {
            _environment = environment;

            Color = color;
            _renderHelper = new RenderHelper();
        }

        public int RequiredWidth => 1;

        public int RequiredHeight => 1;

        public bool Changed => false;

        public UiEnvironment Environment => _environment;

        public Texture2D Render(Graphics graphics, int width, int height)
        {
            _renderHelper.Begin(graphics, width, height);

            if (_renderHelper.SizeChanged)
            {
                _renderHelper.BeginSpriteBatchDraw();
                graphics.Device.Clear(Color);
                _renderHelper.FinishSpriteBatchDraw();
            }

            return _renderHelper.Result;
        }
        public void Update(Vector2 position, int width, int height) { }
    }
}
