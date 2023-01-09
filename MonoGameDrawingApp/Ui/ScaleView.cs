using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameDrawingApp.Ui
{
    public class ScaleView : IUiElement
    {
        public IUiElement Child;

        private readonly RenderHelper _renderHelper;

        public ScaleView(IUiElement child)
        {
            Child = child;
            _renderHelper = new RenderHelper();
        }

        public int RequiredWidth => 1;

        public int RequiredHeight => 1;

        public bool Changed => Child.Changed;

        public Texture2D Render(Graphics graphics, int width, int height)
        {
            _renderHelper.Begin(graphics, width, height);

            if (Changed || _renderHelper.SizeChanged)
            {
                Texture2D childRender = Child.Render(graphics, Child.RequiredWidth, Child.RequiredHeight);

                _renderHelper.BeginDraw();

                graphics.SpriteBatch.Draw(
                    texture: childRender,
                    destinationRectangle: new Rectangle(0, 0, width, height),
                    color: Color.White
                );

                _renderHelper.FinishDraw();
            }

            return _renderHelper.Result;
        }

        public void Update(Vector2 position, int width, int height)
        {
            Child.Update(position, width, height);
        }
    }
}
