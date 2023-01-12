using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameDrawingApp.Ui.Buttons;
using System;

namespace MonoGameDrawingApp.Ui.Popup.ContextMenu.Items
{
    public class ContextMenuButton : IUiElement
    {
        public readonly Action OnClick;
        public bool Disabled;

        private readonly UiEnvironment _environment;

        private readonly Button _button;
        private readonly ColorModifier _colorModifier;
        private readonly TextView _textView;

        public ContextMenuButton(UiEnvironment environment, string title, Action onClick)
        {
            _environment = environment;

            OnClick = onClick;

            _textView = new TextView(environment, title);
            _colorModifier = new ColorModifier(environment, _textView, Color.Black);
            _button = new Button(environment, _colorModifier);
        }

        public bool Changed => _button.Changed;

        public int RequiredWidth => _button.RequiredWidth;

        public int RequiredHeight => _button.RequiredHeight;

        public UiEnvironment Environment => _environment;

        public Texture2D Render(Graphics graphics, int width, int height)
        {
            return _button.Render(graphics, width, height);
        }

        public void Update(Vector2 position, int width, int height)
        {
            _button.Update(position, width, height);
            _colorModifier.Color = Disabled ? Color.DarkGray : (_button.ContainsMouse ? Color.White : Color.Black);
            if (_button.JustLeftClicked)
            {
                OnClick();
            }
        }
    }
}
