using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public Texture2D Render(Graphics graphics, Vector2 position, int width, int height)
        {
            Texture2D childRender = Child.Render(graphics, position, Child.RequiredWidth, Child.RequiredHeight);
            _renderHelper.Begin(graphics, width, height);

            graphics.SpriteBatch.Draw(
                texture: childRender,
                destinationRectangle: new Rectangle(0, 0, width, height),
                color: Color.White
            );

            return _renderHelper.Finish();
        }
    }
}
