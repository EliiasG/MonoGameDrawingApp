using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameDrawingApp.Ui
{
    public class CenterView : IUiElement
    {
        public readonly IUiElement Child;
        public readonly bool CenterVertical;
        public readonly bool CenterHorizontal;

        private readonly RenderHelper _renderHelper;

        public CenterView(IUiElement child, bool centerHorizontal, bool centerVertical)
        {
            Child = child;
            CenterVertical = centerVertical;
            CenterHorizontal = centerHorizontal;

            _renderHelper = new RenderHelper();
        }

        public bool Changed => Child.Changed;

        public int RequiredWidth => Child.RequiredWidth;

        public int RequiredHeight => Child.RequiredHeight;

        public Texture2D Render(Graphics graphics, int width, int height)
        {
            _renderHelper.Begin(graphics, width, height);

            if (Changed || _renderHelper.SizeChanged)
            {
                Vector2 position = new Vector2(CenterHorizontal ? width / 2 - Child.RequiredWidth / 2 : 0, CenterVertical ? height / 2 - Child.RequiredHeight / 2 : 0);
                Point size = new Point(CenterHorizontal ? Child.RequiredWidth : width, CenterVertical ? Child.RequiredHeight : height);

                Texture2D render = Child.Render(graphics, size.X, size.Y);

                _renderHelper.BeginDraw();

                graphics.SpriteBatch.Draw(
                    texture: render,
                    position: position,
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
