using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MonoGameDrawingApp.Ui.Base.Buttons
{
    public class CheckButton : IUiElement
    {
        private bool _isChecked;
        private bool _isDisabled;

        private readonly IUiElement _disabled;
        private readonly IUiElement _enabled;
        private readonly ChangeableView _icon;
        private readonly ColorModifier _colorModifier;
        private readonly SmartButton _button;

        public CheckButton(UiEnvironment environment, bool isChecked, bool isDisabled = false)
        {
            _isChecked = isChecked;
            _isDisabled = isDisabled;
            Environment = environment;

            _disabled = new SpriteView(Environment, "icons/disabled");
            _enabled = new SpriteView(Environment, "icons/enabled");

            _icon = new ChangeableView(Environment, _disabled);

            _colorModifier = new ColorModifier(environment, new ScaleView(environment, _icon), Environment.Theme.ButtonColor);

            _button = new SmartButton(Environment, _colorModifier, () => IsChecked ^= true);
        }

        public bool IsChecked
        {
            get => _isChecked;
            set
            {
                if (!_isDisabled && _isChecked != value)
                {
                    _isChecked = value;
                    CheckedChanged?.Invoke(_isChecked);
                }
            }
        }

        public bool IsDisabled
        {
            get => _isDisabled;
            set
            {
                _isDisabled = value;
                if (_isDisabled)
                {
                    _isChecked = false;
                }
            }
        }

        public Action<bool> CheckedChanged { get; set; }

        public bool Changed => _button.Changed;

        public int RequiredWidth => _button.RequiredWidth;

        public int RequiredHeight => _button.RequiredHeight;

        public UiEnvironment Environment { get; init; }

        public Texture2D Render(Graphics graphics, int width, int height)
        {
            return _button.Render(graphics, width, height);
        }

        public void Update(Vector2 position, int width, int height)
        {
            _button.Update(position, width, height);
            _icon.Child = _isChecked ? _enabled : _disabled;
            _colorModifier.Color = _isDisabled ?
                Environment.Theme.ButtonColor :
                (_button.Button.ContainsMouse ?
                    Environment.Theme.HoveringButtonColor :
                    Environment.Theme.SelectedButtonColor);
        }
    }
}
