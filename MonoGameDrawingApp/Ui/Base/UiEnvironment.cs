using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameDrawingApp.Ui.Base.Themes;
using System;

namespace MonoGameDrawingApp.Ui.Base
{
    public class UiEnvironment
    {
        public readonly Graphics Graphics;
        public readonly ITheme Theme;
        public readonly float FontHeight;
        public readonly ContentManager Content;

        public KeyboardState OldKeyboardState;
        public IUiElement Root;
        public object Clipboard = null; //Maybe bad? the convention says not to create empty interfaces, and use attributes instead, but that does not seem to work for variable types

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

        public UiEnvironment(Graphics graphics, ITheme theme, SpriteFont font, ContentManager content)
        {
            Graphics = graphics;
            Theme = theme;
            Font = font;
            FontHeight = font.MeasureString("X").Y;
            Content = content;
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
            //Debug.WriteLine(Root.Changed);
            Texture2D render = Root.Render(Graphics, Graphics.Device.Viewport.Width, Graphics.Device.Viewport.Height);
            Mouse.SetCursor(Cursor);

            Graphics.Device.Clear(Theme.BackgroundColor);
            Graphics.SpriteBatch.Begin();

            Graphics.SpriteBatch.Draw(
                texture: render,
                position: new Vector2(0),
                color: Color.White
            );

            _updateShortCuts();

            OldKeyboardState = Keyboard.GetState();

            Graphics.SpriteBatch.End();
        }
        public bool JustPressed(Keys key)
        {
            return Keyboard.GetState().IsKeyDown(key) && OldKeyboardState.IsKeyUp(key);
        }
        private void _updateShortCuts()
        {
            KeyboardState keyboardState = Keyboard.GetState();

            bool ctrlShift = keyboardState.IsKeyDown(Keys.LeftControl) && keyboardState.IsKeyDown(Keys.LeftShift);

            if (ctrlShift && keyboardState.IsKeyDown(Keys.G) && OldKeyboardState.IsKeyUp(Keys.G))
            {
                GC.Collect();
            }
        }
    }
}
