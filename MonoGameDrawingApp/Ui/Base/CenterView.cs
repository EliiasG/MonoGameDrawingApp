using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameDrawingApp.Ui.Base
{
    public class CenterView : IUiElement
    {
        private readonly RenderHelper _renderHelper;

        public CenterView(UiEnvironment environment, IUiElement child, bool centerHorizontal, bool centerVertical)
        {
            Environment = environment;

            Child = child;
            CenterVertical = centerVertical;
            CenterHorizontal = centerHorizontal;

            _renderHelper = new RenderHelper();
        }

        public bool Changed => Child.Changed;

        public int RequiredWidth => Child.RequiredWidth;

        public int RequiredHeight => Child.RequiredHeight;

        public UiEnvironment Environment { get; }

        public bool CenterVertical { get; }

        public bool CenterHorizontal { get; }

        public IUiElement Child { get; }

        private Vector2 CalculateChildPosition(int width, int height)
        {
            return new Vector2(CenterHorizontal ? (width / 2) - (Child.RequiredWidth / 2) : 0, CenterVertical ? (height / 2) - (Child.RequiredHeight / 2) : 0);
        }

        private Point CalculateChildSize(int width, int height)
        {
            return new Point(CenterHorizontal ? Child.RequiredWidth : width, CenterVertical ? Child.RequiredHeight : height);
        }

        public Texture2D Render(Graphics graphics, int width, int height)
        {
            _renderHelper.Begin(graphics, width, height);

            if (Changed || _renderHelper.SizeChanged)
            {
                Vector2 position = CalculateChildPosition(width, height);
                Point size = CalculateChildSize(width, height);

                Texture2D render = Child.Render(graphics, size.X, size.Y);

                _renderHelper.BeginSpriteBatchDraw();

                graphics.SpriteBatch.Draw(
                    texture: render,
                    position: position,
                    color: Color.White
                );

                _renderHelper.FinishSpriteBatchDraw();
            }

            return _renderHelper.Result;
        }

        public void Update(Vector2 position, int width, int height)
        {
            Point size = CalculateChildSize(width, height);
            Vector2 childPosition = CalculateChildPosition(width, height);
            Child.Update(position + childPosition, size.X, size.Y);
        }
    }
}
