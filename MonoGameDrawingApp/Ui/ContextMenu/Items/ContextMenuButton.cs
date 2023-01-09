using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MonoGameDrawingApp.Ui.ContextMenu.Items
{
    public class ContextMenuButton : IUiElement
    {
        public readonly Action OnClick;

        private readonly Button _button;
        private readonly ColorModifier _colorModifier;
        private readonly TextView _textView;

        public ContextMenuButton(string title, Action onClick, SpriteFont font)
        {
            OnClick = onClick;

            _textView = new TextView(font, title);
            _colorModifier = new ColorModifier(_textView, Color.Black);
            _button = new Button(_colorModifier);
        }

        public bool Changed => _button.Changed;

        public int RequiredWidth => _button.RequiredWidth;

        public int RequiredHeight =>_button.RequiredHeight;

        public Texture2D Render(Graphics graphics, int width, int height)
        {
            return _button.Render(graphics, width, height);
        }

        public void Update(Vector2 position, int width, int height)
        {
            _button.Update(position, width, height);
            _colorModifier.Color = _button.ContainsMouse ? Color.White : Color.Black;
            if(_button.JustLeftClicked)
            {
                OnClick();
            }
        }
    }
}
