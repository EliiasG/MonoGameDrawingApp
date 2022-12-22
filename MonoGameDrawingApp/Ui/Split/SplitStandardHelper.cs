using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameDrawingApp.Ui.Split
{
    internal class SplitStandardHelper
    {
        private RenderHelper _renderHelper = new RenderHelper();
        public SplitStandardHelper()
        {
            _renderHelper = new RenderHelper();
        }

        public Texture2D Render(Graphics graphics, Vector2 position, int width, int height, Vector2 secondPosition, IUiElement first, IUiElement second)
        {
            Texture2D firstRender = first.Render(graphics, position, secondPosition.X == 0 ? width : (int)secondPosition.X, secondPosition.Y == 0 ? height : (int)secondPosition.Y);
            Texture2D secondRender = second.Render(graphics, position + secondPosition, secondPosition.X == 0 ? width : (width - (int)secondPosition.X), secondPosition.Y == 0 ? height : (height - (int)secondPosition.Y));

            _renderHelper.Begin(graphics, width, height);

            graphics.SpriteBatch.Draw(
                texture: firstRender,
                position: new Vector2(0),
                color: Color.White
            );

            graphics.SpriteBatch.Draw(
                texture: secondRender,
                position: secondPosition,
                color: Color.White
            );

            return _renderHelper.Finish();
        }
    }
}
