using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameDrawingApp.Ui
{
    public class ColorRect : IUiElement
    {
        public readonly Color Color;
        private RenderHelper _renderHelper;

        public ColorRect(Color color)
        {
            Color = color;
            _renderHelper = new RenderHelper();
        }

        public int RequiredWidth => 1;

        public int RequiredHeight => 1;

        public bool Changed => false;

        public Texture2D Render(Graphics graphics, int width, int height)
        {
            _renderHelper.Begin(graphics, width, height);

            if (_renderHelper.SizeChanged)
            {
                _renderHelper.BeginDraw();
                graphics.Device.Clear(Color);
                _renderHelper.FinishDraw();
            }

            return _renderHelper.Result;
        }
        public void Update(Vector2 position, int width, int height) {}
    }
}
