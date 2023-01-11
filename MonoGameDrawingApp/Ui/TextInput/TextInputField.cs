﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameDrawingApp.Ui.Split.Horizontal;
using MonoGameDrawingApp.Ui.TextInput.Filters.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoGameDrawingApp.Ui.TextInput
{
    public class TextInputField : IUiElement
    {
        public string Value { get; set; }

        public bool IsSelected { get; set; }

        public bool IsSelectable { get; set; }
        public bool IsDeselectable { get; set; }

        public readonly int MaxLength;

        public SpriteFont Font;

        public readonly ITextInputFilter[] Filters;

        private int _cursorPosition;

        private readonly Dictionary<Keys, char> _customKeyChars;
        private readonly Dictionary<char, char> _customShiftVersions;

        private readonly IUiElement _background;
        private readonly IUiElement _backgroundSelected;

        private readonly IUiElement _cursorOn;
        private readonly IUiElement _cursorOff;

        private ISet<Keys> _oldKeys;

        private readonly Button _button;
        private readonly StackView _outer;
        private readonly ChangeableView _currentBrackground;
        private readonly StackView _inner;
        private readonly TextView _textView;
        private readonly ChangeableView _currentCursor;
        private readonly HSplit _cursorInner;
        private readonly HSplit _cursorOuter;

        public TextInputField(SpriteFont font, string value, ITextInputFilter[] filters, bool isSelectable = true, bool isDeselectable = true, int maxLength = -1)
        {
            Value = value;
            IsSelectable = isSelectable;
            IsDeselectable = isDeselectable;
            Filters = filters;
            Font = font;
            MaxLength = maxLength;
            _oldKeys = new HashSet<Keys>();

            IsSelected = false;

            _customKeyChars = _createCustomKeyChars();
            _customShiftVersions = _createCustomShiftVersions();

            /* 
             * Simplified Structure:
             * 
             * _button:
             *   _outer:
             *     _currentBackground
             *     CenterView:
             *       MinSize:
             *         _inner:
             *           _textView
             *           _cursorOuter:
             *             Empty
             *             _cursorInner:
             *               _currentCursor
             *               Empty
             */
            _background = new ColorRect(Color.Gray);
            _backgroundSelected = new ColorRect(Color.LightGray);
            _cursorOn = new ColorRect(Color.White);
            _cursorOff = new ColorRect(Color.Transparent);

            _currentCursor = new ChangeableView(_cursorOn);

            _cursorInner = new HSplitStandard(new MinSize(_currentCursor, 2, 1), new ColorRect(Color.Transparent), 0);

            _cursorOuter = new HSplitStandard(new ColorRect(Color.Transparent), _cursorInner, 0);
            _textView = new TextView(Font, Value);

            _inner = new StackView(new List<IUiElement>() { _textView, _cursorOuter });

            _currentBrackground = new ChangeableView(_background);

            _outer = new StackView(new List<IUiElement>()
            {
                _currentBrackground,
                new CenterView(new MinSize(_inner, 0, (int) Font.MeasureString("X").Y), false, true),
            });

            _button = new Button(_outer);
        }

        public bool Changed => _button.Changed;

        public int RequiredWidth => _button.RequiredWidth;

        public int RequiredHeight => _button.RequiredHeight;

        public Texture2D Render(Graphics graphics, int width, int height)
        {
            return _button.Render(graphics, width, height);
        }

        public void Update(Vector2 position, int width, int height)
        {
            if (IsSelected)
            {
                _updateTyping();
                if (IsDeselectable && Mouse.GetState().LeftButton == ButtonState.Pressed && !_button.ContainsMouse)
                {
                    IsSelected = false;
                }
            }
            else if (IsSelectable && _button.JustLeftClicked)
            {
                IsSelected = true;
            }
            else
            {
                _cursorPosition = Value.Length;
            }

            _updateUiState();
            _button.Update(position, width, height);
        }

        private void _updateUiState()
        {
            _textView.Text = Value;
            _currentBrackground.Child = IsSelected ? _backgroundSelected : _background;
            _currentCursor.Child = IsSelected ? _cursorOn : _cursorOff;
            _cursorOuter.SplitPosition = (int) Font.MeasureString(Value.Substring(0, _cursorPosition)).X;
            _cursorInner.SplitPosition = 0;
        }

        private void _updateTyping()
        {
            KeyboardState _keyboard = Keyboard.GetState();
            Keys[] keys = _keyboard.GetPressedKeys();

            if (keys.Contains(Keys.Left) && !_oldKeys.Contains(Keys.Left))
            {
                _cursorPosition = Math.Max(_cursorPosition - 1, 0);
            }
            else if (keys.Contains(Keys.Right) && !_oldKeys.Contains(Keys.Right))
            {
                _cursorPosition = Math.Min(_cursorPosition + 1, Value.Length);
            }
            if (keys.Contains(Keys.Back) && !_oldKeys.Contains(Keys.Back) && _cursorPosition > 0)
            {
                Value = Value.Remove(--_cursorPosition, 1);
            }

            if (MaxLength != -1 && Value.Length >= MaxLength)
            {
                _oldKeys = keys.ToHashSet();
                return;
            }

            foreach (Keys key in keys)
            {
                if (_oldKeys.Contains(key)) continue;

                string keyName = _customKeyChars.ContainsKey(key) ? _customKeyChars[key].ToString() : key.ToString();
                if (keyName.Length > 1) continue;

                bool shift = _keyboard.IsKeyDown(Keys.LeftShift);
                bool isUpper = shift ^ _keyboard.CapsLock;

                char keyChar = (isUpper ? keyName : keyName.ToLower()).First();

                keyChar = _customShiftVersions.ContainsKey(keyChar) && shift ? _customShiftVersions[keyChar] : keyChar;

                if (!_checkFilters(keyChar, Filters)) continue;

                _typeChar(keyChar);

            }

            _oldKeys = keys.ToHashSet();
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
                {Keys.OemMinus, '-'}
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

    }
}