using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameDrawingApp.Ui.Base.Buttons;
using MonoGameDrawingApp.Ui.Base.Themes;
using System;

namespace MonoGameDrawingApp.Ui.Base.Popup.ContextMenus.Items
{
    public class ContextMenuButton : IUiElement
    {
        private readonly Button _button;
        private readonly ColorModifier _colorModifier;
        private readonly TextView _textView;

        public ContextMenuButton(UiEnvironment environment, string title, Action onClick)
        {
            Environment = environment;

            OnClick = onClick;

            _textView = new TextView(environment, title);
            _colorModifier = new ColorModifier(environment, _textView, Theme.DefaultTextColor);
            _button = new Button(environment, _colorModifier);
        }

        public string Title
        {
            get => _textView.Text;
            set => _textView.Text = value;
        }

        public bool Changed => _button.Changed;

        public int RequiredWidth => _button.RequiredWidth;

        public int RequiredHeight => _button.RequiredHeight;

        public UiEnvironment Environment { get; }

        private ITheme Theme => Environment.Theme;

        public Action OnClick { get; }
        public bool Disabled { get; set; }

        public Texture2D Render(Graphics graphics, int width, int height)
        {
            return _button.Render(graphics, width, height);
        }

        public void Update(Vector2 position, int width, int height)
        {
            _colorModifier.Color = Disabled ? Theme.SelectedButtonColor : _button.ContainsMouse ? Theme.HoveringTextColor : Theme.DefaultTextColor;
            if (_button.JustLeftClicked && !Disabled)
            {
                OnClick();
            }
            _button.Update(position, width, height);
        }
    }
}
