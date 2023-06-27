using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameDrawingApp.Ui.Base.Themes;
using System.Collections.Generic;
using System.Linq;

namespace MonoGameDrawingApp.Ui.Base
{
    public class UiEnvironment
    {


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

        public SpriteFont Font { get; }

        public MouseState OldMouse { get; set; }

        public KeyboardState OldKeyboardState { get; set; }

        public IUiElement Root { get; set; }

        public object Clipboard { get; set; } //Maybe bad? the convention says not to create empty interfaces, and use attributes instead, but that does not seem to work for variable types

        public Graphics Graphics { get; }

        public ITheme Theme { get; }

        public float FontHeight { get; }

        public ContentManager Content { get; }

        private MouseCursor _cursor;
        private readonly List<GlobalShortcut> _globalShortcuts;
        private bool _cursorLocked;

        public UiEnvironment(Graphics graphics, ITheme theme, SpriteFont font, ContentManager content)
        {
            Graphics = graphics;
            Theme = theme;
            Font = font;
            FontHeight = font.MeasureString("X").Y;
            _globalShortcuts = new List<GlobalShortcut>();
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

            UpdateShortcuts();

            OldKeyboardState = Keyboard.GetState();
            OldMouse = Mouse.GetState();

            Graphics.SpriteBatch.End();
        }
        public bool JustPressed(Keys key)
        {
            return Keyboard.GetState().IsKeyDown(key) && OldKeyboardState.IsKeyUp(key);
        }

        public void AddShortcut(GlobalShortcut shortcut)
        {
            _globalShortcuts.Add(shortcut);
        }

        public void RemoveShortcut(GlobalShortcut shortcut)
        {
            _globalShortcuts.Remove(shortcut);
        }

        private void UpdateShortcuts()
        {
            KeyboardState keyboardState = Keyboard.GetState();

            foreach (GlobalShortcut shortcut in _globalShortcuts)
            {
                bool allDown = shortcut.ActivationKeys.All(keyboardState.IsKeyDown);
                bool anyJustPressed = shortcut.ActivationKeys.Any(OldKeyboardState.IsKeyUp);
                if (allDown && anyJustPressed)
                {
                    shortcut.Run();
                }
            }
        }
    }
}
