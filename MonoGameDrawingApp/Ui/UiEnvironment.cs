using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameDrawingApp.Ui.Themes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameDrawingApp.Ui
{
    public class UiEnvironment
    {
        public readonly IUiElement Root;

        public readonly Graphics Graphics;

        public readonly ITheme Theme;

        public readonly SpriteFont Font;

        public UiEnvironment(IUiElement root, Graphics graphics, ITheme theme, SpriteFont font)
        {
            Root = root;
            Graphics = graphics;
            Theme = theme;
            Font = font;
        }

        public void Render()
        {
            Graphics.Device.Clear(Theme.BackgroundColor);

            Texture2D render = Root.Render(Graphics, Graphics.Device.Viewport.Width, Graphics.Device.Viewport.Height);
            Graphics.SpriteBatch.Begin();

            Graphics.SpriteBatch.Draw(
                texture: render,
                position: new Vector2(0),
                color: Color.White
            );

            Graphics.SpriteBatch.End();
        }
    }
}
