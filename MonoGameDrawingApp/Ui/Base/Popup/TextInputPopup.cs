using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameDrawingApp.Ui.Base;
using MonoGameDrawingApp.Ui.Base.TextInput.Filters.Alphanumeric;
using MonoGameDrawingApp.Ui.Buttons;
using MonoGameDrawingApp.Ui.List;
using MonoGameDrawingApp.Ui.TextInput;
using System;
using System.Collections.Generic;

namespace MonoGameDrawingApp.Ui.Popup
{
    public class TextInputPopup : IUiElement
    {
        public readonly Action<string> Confirmed;
        public readonly PopupEnvironment PopupEnvironment;

        private MouseState _oldMouse;

        private readonly UiEnvironment _environment;

        private readonly MinSize _outer;
        private readonly TextInputField _textInput;
        private readonly TextButton _confirmButton;
        private readonly TextButton _cancelButton;
        private bool _done = false;

        public TextInputPopup(UiEnvironment environment, PopupEnvironment popupEnvironment, Action<string> confirmed, ITextInputFilter[] filters, string title, string currentValue = "")
        {
            _environment = environment;
            _textInput = new TextInputField(environment, currentValue, filters, true, false);
            _textInput.IsSelected = true;
            _confirmButton = new TextButton(environment, "Confirm");
            _cancelButton = new TextButton(environment, "Cancel");
            Confirmed = confirmed;
            PopupEnvironment = popupEnvironment;

            /*
             * Structure:
             * 
             * 
             * _outer:
             *   StackView:
             *     ColorRect
             *     CenterView:
             *       VListView
             *         CenterView: 
             *           ColorModifier: TextView
             *         HListView:
             *           MinSize: Empty
             *           MinSize: _textInput
             *           MinSize: Empty
             *         MinSize: Empty
             *         CenterView:
             *           HListView:
             *             _cancelButton
             *             MinSise: Empty
             *             _confirmButton
             */

            _outer = new MinSize(
                environment: environment,
                child: new StackView(
                    environment: environment,
                    children: new IUiElement[]
                    {
                        new ColorRect(environment, environment.Theme.SecondaryMenuBackgroundColor),
                        new CenterView(
                            environment: environment,
                            child: new VListView<IUiElement>(
                                environment: environment,
                                items: new List<IUiElement>
                                {
                                    new CenterView(
                                        environment: environment,
                                        child: new ColorModifier(environment, new TextView(environment, title), environment.Theme.DefaultTextColor),
                                        centerHorizontal: true,
                                        centerVertical: false
                                    ),
                                    new HListView<IUiElement>(
                                        environment: environment,
                                        items: new List<IUiElement>
                                        {
                                            new MinSize(environment, new ColorRect(environment, Color.Transparent), 20, 1),
                                            new MinSize(environment, _textInput, 200, 40),
                                            new MinSize(environment, new ColorRect(environment, Color.Transparent), 20, 1),
                                        }
                                    ),
                                    new MinSize(environment, new ColorRect(environment, Color.Transparent), 1, 20),
                                    new CenterView(
                                        environment: environment,
                                        child: new HListView<IUiElement>(
                                            environment: environment,
                                            items: new List<IUiElement>
                                            {
                                                _cancelButton,
                                                new MinSize(environment, new ColorRect(environment, Color.Transparent), 20, 1),
                                                _confirmButton,
                                            }
                                        ),
                                        centerHorizontal: true,
                                        centerVertical: true
                                    ),
                                }
                            ),
                            centerHorizontal: false,
                            centerVertical: true
                        ),
                    }
                ),
                width: 1, //set by textinput
                height: 130
            );
        }

        public bool Changed => _outer.Changed;

        public int RequiredWidth => _outer.RequiredWidth;

        public int RequiredHeight => _outer.RequiredHeight;

        public UiEnvironment Environment => _environment;

        public Texture2D Render(Graphics graphics, int width, int height)
        {
            return _outer.Render(graphics, width, height);
        }

        public void Update(Vector2 position, int width, int height)
        {
            if (!_done)
            {
                if (_confirmButton.Button.JustLeftClicked || Keyboard.GetState().IsKeyDown(Keys.Enter))
                {
                    PopupEnvironment.Close();
                    Confirmed(_textInput.Value);
                    _done = true;
                }
                else if (_cancelButton.Button.JustLeftClicked)
                {
                    PopupEnvironment.Close();
                    _done = true;
                }
            }
            _oldMouse = Mouse.GetState();

            _outer.Update(position, width, height);
        }
    }
}
