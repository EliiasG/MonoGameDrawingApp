using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameDrawingApp.Ui.Base.Buttons;
using MonoGameDrawingApp.Ui.Base.Split.Horizontal;
using MonoGameDrawingApp.Ui.Base.TextInput.Filters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoGameDrawingApp.Ui.Base.TextInput
{
    public class TextInputField : IUiElement
    {
        public readonly int MaxLength;

        public readonly ITextInputFilter[] Filters;

        private readonly UiEnvironment _environment;

        private readonly Dictionary<Keys, char> _customKeyChars;
        private readonly Dictionary<char, char> _customShiftVersions;
        private readonly Dictionary<char, char> _customAltVersions;

        private readonly IUiElement _background;
        private readonly IUiElement _backgroundHovering;
        private readonly IUiElement _backgroundSelected;
        private readonly IUiElement _cursorOn;
        private readonly IUiElement _cursorOff;

        private readonly Button _button;
        private readonly StackView _outer;
        private readonly ChangeableView _currentBrackground;
        private readonly StackView _inner;
        private readonly TextView _textView;
        private readonly ChangeableView _currentCursor;
        private readonly HSplit _cursorInner;
        private readonly HSplit _cursorOuter;

        private MouseState _oldMouse;
        private int _cursorPosition;
        private int _counter;
        private string _value;

        public TextInputField(UiEnvironment environment, string value, ITextInputFilter[] filters, bool isSelectable = true, bool isDeselectable = true, bool centerHorizontal = false, int maxLength = -1)
        {
            _environment = environment;

            Value = value;
            _cursorPosition = value.Length;
            IsSelectable = isSelectable;
            IsDeselectable = isDeselectable;
            Filters = filters;
            MaxLength = maxLength;
            _oldMouse = Mouse.GetState();

            IsSelected = false;

            _customKeyChars = _createCustomKeyChars();
            _customShiftVersions = _createCustomShiftVersions();
            _customAltVersions = _createCustomAltVersions();

            /* 
             * Simplified Structure:
             * 
             * _button:
             *   _outer:
             *     _currentBackground
             *     CenterView:
             *       MinSize:
             *         _inner:
             *           ColorModifier:
             *             _textView
             *           _cursorOuter:
             *             Empty
             *             _cursorInner:
             *               _currentCursor
             *               Empty
             */
            _background = new ColorRect(environment, environment.Theme.ButtonColor);
            _backgroundHovering = new ColorRect(environment, environment.Theme.HoveringButtonColor);
            _backgroundSelected = new ColorRect(environment, environment.Theme.SelectedButtonColor);
            _cursorOn = new ColorRect(environment, environment.Theme.EditingTextColor);
            _cursorOff = new ColorRect(environment, Color.Transparent);

            _currentCursor = new ChangeableView(environment, _cursorOn);

            _cursorInner = new HSplitStandard(environment, new MinSize(environment, _currentCursor, 2, 1), new ColorRect(environment, Color.Transparent), 0);

            _cursorOuter = new HSplitStandard(environment, new ColorRect(environment, Color.Transparent), _cursorInner, 0);
            _textView = new TextView(Environment, Value);

            _inner = new StackView(environment, new List<IUiElement>() { new ColorModifier(Environment, _textView, Environment.Theme.EditingTextColor), _cursorOuter });

            _currentBrackground = new ChangeableView(environment, _background);

            _outer = new StackView(environment, new List<IUiElement>()
            {
                _currentBrackground,
                new CenterView(environment, new MinSize(environment, _inner, 0, (int) Environment.FontHeight), centerHorizontal, true),
            });

            _button = new Button(environment, _outer);
        }

        public string Value
        {
            get => _value;
            set
            {
                if (value != _value)
                {
                    _value = value;
                    ValueChanged?.Invoke();
                }
            }
        }

        public string Extention { get; set; }

        public bool IsSelected { get; set; }

        public bool IsSelectable { get; set; }

        public bool IsDeselectable { get; set; }

        public Action TextEntered { get; set; }

        public Action Deselected { get; set; }

        public Action Selected { get; set; }

        public Action ValueChanged { get; set; }

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
            if (IsSelected)
            {
                _updateTyping();
                if (IsDeselectable && (Mouse.GetState().LeftButton == ButtonState.Pressed && !_button.ContainsMouse && _oldMouse.LeftButton == ButtonState.Released || Environment.JustPressed(Keys.Escape)))
                {
                    IsSelected = false;
                    Deselected?.Invoke();
                }
                if (Environment.JustPressed(Keys.Enter))
                {
                    if (IsDeselectable)
                    {
                        IsSelected = false;
                    }
                    TextEntered?.Invoke();
                }
            }
            else if (IsSelectable && _button.JustLeftClicked)
            {
                IsSelected = true;
                Selected?.Invoke();
            }
            else
            {
                _cursorPosition = Value.Length;
            }

            _oldMouse = Mouse.GetState();
            _updateUiState();
            _button.Update(position, width, height);
        }

        private void _updateUiState()
        {
            _cursorPosition = Math.Min(_cursorPosition, Value.Length);
            _counter = (_counter + 1) % 60;
            _textView.Text = Value + Extention;
            _currentBrackground.Child = IsSelected ? _backgroundSelected : _button.ContainsMouse ? _backgroundHovering : _background;
            _currentCursor.Child = IsSelected && _counter < 30 ? _cursorOn : _cursorOff;
            _cursorOuter.SplitPosition = (int)Environment.Font.MeasureString(Value.Substring(0, _cursorPosition)).X;
            _cursorInner.SplitPosition = 0;
        }

        private void _updateTyping()
        {
            KeyboardState _keyboard = Keyboard.GetState();
            Keys[] keys = _keyboard.GetPressedKeys();

            if (Environment.JustPressed(Keys.Left))
            {
                _cursorPosition = Math.Max(_cursorPosition - 1, 0);
                _counter = 0;
            }
            else if (Environment.JustPressed(Keys.Right))
            {
                _cursorPosition = Math.Min(_cursorPosition + 1, Value.Length);
                _counter = 0;
            }
            if (Environment.JustPressed(Keys.Back) && _cursorPosition > 0)
            {
                Value = Value.Remove(--_cursorPosition, 1);
                _counter = 0;
            }

            if (MaxLength != -1 && Value.Length >= MaxLength || keys.Contains(Keys.LeftControl))
            {
                return;
            }

            foreach (Keys key in keys)
            {
                if (!Environment.JustPressed(key)) continue;

                string keyName = _customKeyChars.ContainsKey(key) ? _customKeyChars[key].ToString() : key.ToString();
                if (keyName.Length > 1) continue;

                bool shift = _keyboard.IsKeyDown(Keys.LeftShift);
                bool isUpper = shift != _keyboard.CapsLock;

                char keyChar = (isUpper ? keyName : keyName.ToLower()).First();

                keyChar = _customShiftVersions.ContainsKey(keyChar) && shift ? _customShiftVersions[keyChar] : keyChar;
                keyChar = _customAltVersions.ContainsKey(keyChar) && _keyboard.IsKeyDown(Keys.LeftAlt) ? _customAltVersions[keyChar] : keyChar;

                if (!_checkFilters(keyChar, Filters)) continue;

                _typeChar(keyChar);

            }
        }

        private bool _checkFilters(char key, ITextInputFilter[] filters)
        {
            foreach (ITextInputFilter filter in filters)
            {
                if (filter.AllowedCharacters != null && filter.AllowedCharacters.Contains(key))
                {
                    return true;
                }

                if (filter.SubFilters == null) continue;

                if (_checkFilters(key, filter.SubFilters))
                {
                    return true;
                }
            }

            return false;
        }

        private void _typeChar(char value)
        {
            Value = Value.Insert(_cursorPosition, value.ToString());
            ++_cursorPosition;
            _counter = 0;
        }

        private Dictionary<Keys, char> _createCustomKeyChars()
        {
            return new Dictionary<Keys, char>
            {
                {Keys.D0, '0'},
                {Keys.D1, '1'},
                {Keys.D2, '2'},
                {Keys.D3, '3'},
                {Keys.D4, '4'},
                {Keys.D5, '5'},
                {Keys.D6, '6'},
                {Keys.D7, '7'},
                {Keys.D8, '8'},
                {Keys.D9, '9'},
                {Keys.Subtract, '-'},
                {Keys.OemMinus, '-'},
                {Keys.Space, ' '},
                {Keys.OemPeriod, '.'}
            };
        }

        private Dictionary<char, char> _createCustomShiftVersions()
        {
            return new Dictionary<char, char>
            {
                {'0', '='},
                {'1', '!'},
                {'2', '"'},
                {'3', '#'},
                {'4', '¤'},
                {'5', '%'},
                {'6', '&'},
                {'7', '/'},
                {'8', '('},
                {'9', ')'},
            };
        }

        private Dictionary<char, char> _createCustomAltVersions()
        {
            return new Dictionary<char, char>
            {
                {'0', '}'},
                {'2', '@'},
                {'3', '£'},
                {'4', '$'},
                {'5', '€'},
                {'7', '{'},
                {'8', '['},
                {'9', ']'},
            };
        }

    }
}