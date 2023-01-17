using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameDrawingApp.Ui.Themes;

namespace MonoGameDrawingApp.Ui
{
    public class UiEnvironment
    {
        public IUiElement Root;

        public readonly Graphics Graphics;

        public ITheme Theme;

        public readonly SpriteFont Font;

        public UiEnvironment(Graphics graphics, ITheme theme, SpriteFont font)
        {
            Graphics = graphics;
            Theme = theme;
            Font = font;
        }

        public void Render()
        {
            Root.Update(Vector2.Zero, Graphics.Device.Viewport.Width, Graphics.Device.Viewport.Height);
            Texture2D render = Root.Render(Graphics, Graphics.Device.Viewport.Width, Graphics.Device.Viewport.Height);
            Graphics.Device.Clear(Theme.BackgroundColor);
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
