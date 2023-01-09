using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameDrawingApp.Ui.Split.Horizontal;
using System;
using System.Collections.Generic;

namespace MonoGameDrawingApp.Ui
{
    public class TextInputField : IUiElement
    {
        public string Value { get; set; }

        public bool IsSelected { get; set; }

        public bool IsSelectable { get; set; }
        public bool IsDeselectable { get; set; }

        private readonly IUiElement _background;
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

        public TextInputField(SpriteFont font, string value, bool canBeSelected = true, bool canBeDeselected = true)
        {
            Value = value;
            IsSelectable = canBeSelected;
            IsDeselectable = canBeDeselected;

            IsSelected = false;

            /* 
             * Simplified Structure:
             * 
             * _button:
             *   _outer:
             *     _currentBackground
             *     CenterView:
             *       _inner:
             *         _textView
             *         _cursorOuter:
             *           Empty
             *           _cursorInner:
             *             _currentCursor
             *             Empty
             */

            _background = new ColorRect(Color.Gray);
            _backgroundSelected = new ColorRect(Color.LightGray);
            _cursorOn = new ColorRect(Color.White);
            _cursorOff = new ColorRect(Color.Transparent);

            _currentCursor = new ChangeableView(_cursorOn);

            _cursorInner = new HSplitStandard(new MinSize(_currentCursor, 2, 1), new ColorRect(Color.Transparent), 0);

            _cursorOuter = new HSplitStandard(new ColorRect(Color.Transparent), _cursorInner, 0);
            _textView = new TextView(font, Value);

            _inner = new StackView(new List<IUiElement>() { _textView, _cursorOuter });

            _currentBrackground = new ChangeableView(_background);

            _outer = new StackView(new List<IUiElement>() 
            { 
                _currentBrackground,
                new CenterView(_inner, false, true),
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
            _button.Update(position, width, height);
            //TODO continue here
        }
    }
}
