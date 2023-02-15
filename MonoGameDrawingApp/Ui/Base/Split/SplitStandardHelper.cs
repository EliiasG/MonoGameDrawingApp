using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameDrawingApp.Ui.Base.Split
{
    internal class SplitStandardHelper
    {
        private readonly RenderHelper _renderHelper = new();
        public SplitStandardHelper()
        {
            _renderHelper = new RenderHelper();
        }

        public Texture2D Render(Graphics graphics, bool changed, int width, int height, Vector2 secondPosition, IUiElement first, IUiElement second)
        {
            _renderHelper.Begin(graphics, width, height);

            if (changed || _renderHelper.SizeChanged)
            {
                Texture2D firstRender = first.Render(graphics, secondPosition.X == 0 ? width : (int)secondPosition.X, secondPosition.Y == 0 ? height : (int)secondPosition.Y);
                Texture2D secondRender = second.Render(graphics, secondPosition.X == 0 ? width : width - (int)secondPosition.X, secondPosition.Y == 0 ? height : height - (int)secondPosition.Y);

                _renderHelper.BeginDraw();

                graphics.SpriteBatch.Draw(
                    texture: firstRender,
                    position: Vector2.Zero,
                    color: Color.White
                );

                graphics.SpriteBatch.Draw(
                    texture: secondRender,
                    position: secondPosition,
                    color: Color.White
                );

                _renderHelper.FinishDraw();
            }

            return _renderHelper.Result;
        }
    }
}
