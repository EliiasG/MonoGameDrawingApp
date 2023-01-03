using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameDrawingApp.Ui
{
    public class TextView : IUiElement
    {
        public SpriteFont Font;
        public string Text;
        public Color Color;

        private RenderHelper _renderHelper;

        public TextView(SpriteFont font, string text, Color color = new Color())
        {
            Font = font;
            _renderHelper = new RenderHelper();
            Text = text;
            Color = color;
        }

        public int RequiredWidth => (int)Font.MeasureString(Text).X;

        public int RequiredHeight => (int)Font.MeasureString(Text).Y;

        public Texture2D Render(Graphics graphics, Vector2 position, int width, int height)
        {
            _renderHelper.Begin(graphics, width, height);
            graphics.SpriteBatch.DrawString(
                spriteFont: Font,
                text: Text,
                position: Vector2.Zero,
                color: Color.White
            );
            return _renderHelper.FinishDraw();
        }
    }
}
