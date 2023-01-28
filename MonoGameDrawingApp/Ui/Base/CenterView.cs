using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameDrawingApp.Ui.Base;

namespace MonoGameDrawingApp.Ui
{
    public class CenterView : IUiElement
    {
        public readonly IUiElement Child;
        public readonly bool CenterVertical;
        public readonly bool CenterHorizontal;

        private readonly UiEnvironment _environment;

        private readonly RenderHelper _renderHelper;

        public CenterView(UiEnvironment environment, IUiElement child, bool centerHorizontal, bool centerVertical)
        {
            _environment = environment;

            Child = child;
            CenterVertical = centerVertical;
            CenterHorizontal = centerHorizontal;

            _renderHelper = new RenderHelper();
        }

        public bool Changed => Child.Changed;

        public int RequiredWidth => Child.RequiredWidth;

        public int RequiredHeight => Child.RequiredHeight;

        public UiEnvironment Environment => _environment;

        private Vector2 _calculateChildPosition(int width, int height)
        {
            return new Vector2(CenterHorizontal ? width / 2 - Child.RequiredWidth / 2 : 0, CenterVertical ? height / 2 - Child.RequiredHeight / 2 : 0);
        }

        private Point _calculateChildSize(int width, int height)
        {
            return new Point(CenterHorizontal ? Child.RequiredWidth : width, CenterVertical ? Child.RequiredHeight : height);
        }

        public Texture2D Render(Graphics graphics, int width, int height)
        {
            _renderHelper.Begin(graphics, width, height);

            if (Changed || _renderHelper.SizeChanged)
            {
                Vector2 position = _calculateChildPosition(width, height);
                Point size = _calculateChildSize(width, height);

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
            Point size = _calculateChildSize(width, height);
            Vector2 childPosition = _calculateChildPosition(width, height);
            Child.Update(position + childPosition, size.X, size.Y);
        }
    }
}
