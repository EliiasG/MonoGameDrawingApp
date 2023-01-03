using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameDrawingApp.Ui
{
    public class PeekView : IUiElement
    {
        public Vector2 Position = Vector2.Zero;
        public IUiElement Child;

        private RenderHelper _renderHelper;

        public PeekView(IUiElement child)
        {
            _renderHelper = new RenderHelper();
            Child = child;
        }

        public int RequiredWidth => 1;

        public int RequiredHeight => 1;

        public Texture2D Render(Graphics graphics, Vector2 position, int width, int height)
        {
            Texture2D childRender = Child.Render(graphics, position - Position, Math.Max(width, Child.RequiredWidth), Math.Max(height, Child.RequiredHeight));

            _renderHelper.Begin(graphics, width, height);

            graphics.SpriteBatch.Draw(
                texture: childRender,
                position: -Position,
                color: Color.White
            );

            return _renderHelper.FinishDraw();
        }
    }
}
