using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameDrawingApp.Ui.Themes;
using System.Diagnostics;

namespace MonoGameDrawingApp.Ui
{
    public class UiEnvironment
    {
        public IUiElement Root;

        public readonly Graphics Graphics;

        public ITheme Theme;

        public MouseCursor Cursor
        {
            set 
            {
                if (!_cursorLocked)
                {
                    _cursor = value;
                }
            }
            get => _cursor; 
        }

        public readonly SpriteFont Font;

        private MouseCursor _cursor;
        private bool _cursorLocked;

        public UiEnvironment(Graphics graphics, ITheme theme, SpriteFont font)
        {
            Graphics = graphics;
            Theme = theme;
            Font = font;
        }

        public void LockCursor()
        {
            _cursorLocked = true;
        }

        public void Render()
        {
            Cursor = MouseCursor.Arrow;

            _cursorLocked = false;

            Root.Update(Vector2.Zero, Graphics.Device.Viewport.Width, Graphics.Device.Viewport.Height);
            Texture2D render = Root.Render(Graphics, Graphics.Device.Viewport.Width, Graphics.Device.Viewport.Height);
            Debug.WriteLine(Root.Changed);
            Mouse.SetCursor(Cursor);

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
