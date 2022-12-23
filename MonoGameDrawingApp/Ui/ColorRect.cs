using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameDrawingApp.Ui
{
    public class ColorRect : IUiElement
    {
        public Color Color;
        private RenderHelper _renderHelper;

        public ColorRect(Color color)
        {
            Color = color;
            _renderHelper = new RenderHelper();
        }

        public int RequiredWidth => 1;

        public int RequiredHeight => 1;

        public Texture2D Render(Graphics graphics, Vector2 position, int width, int height)
        {
            _renderHelper.Begin(graphics, width, height);
            graphics.Device.Clear(Color);
            return _renderHelper.Finish();
        }
    }
}
