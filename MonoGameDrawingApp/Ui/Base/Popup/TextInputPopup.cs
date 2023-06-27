using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameDrawingApp.Ui.Base.Buttons;
using MonoGameDrawingApp.Ui.Base.Lists;
using MonoGameDrawingApp.Ui.Base.TextInput;
using MonoGameDrawingApp.Ui.Base.TextInput.Filters;
using System;
using System.Collections.Generic;

namespace MonoGameDrawingApp.Ui.Base.Popup
{
    public class TextInputPopup : IUiElement
    {
        private readonly MinSize _outer;
        private readonly TextInputField _textInput;
        private readonly TextButton _confirmButton;
        private readonly TextButton _cancelButton;
        private bool _done;

        public TextInputPopup(UiEnvironment environment, PopupEnvironment popupEnvironment, Action<string> confirmed, ITextInputFilter[] filters, string title, string currentValue = "")
        {
            Environment = environment;
            _textInput = new TextInputField(environment, currentValue, filters, true, false)
            {
                IsSelected = true
            };
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
                                            new EmptySpace(Environment,  20, 1),
                                            new MinSize(environment, _textInput, 200, 40),
                                            new EmptySpace(Environment,  20, 1),
                                        }
                                    ),
                                    new EmptySpace(Environment,  1, 20),
                                    new CenterView(
                                        environment: environment,
                                        child: new HListView<IUiElement>(
                                            environment: environment,
                                            items: new List<IUiElement>
                                            {
                                                _cancelButton,
                                                new EmptySpace(Environment,  20, 1),
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

        public UiEnvironment Environment { get; }

        public Action<string> Confirmed { get; }

        public PopupEnvironment PopupEnvironment { get; }

        public Texture2D Render(Graphics graphics, int width, int height)
        {
            return _outer.Render(graphics, width, height);
        }

        public void Update(Vector2 position, int width, int height)
        {
            if (!_done)
            {
                if (_confirmButton.Button.JustLeftClicked || Environment.JustPressed(Keys.Enter))
                {
                    PopupEnvironment.Close();
                    Confirmed(_textInput.Value);
                    _done = true;
                }
                else if (_cancelButton.Button.JustLeftClicked || Environment.JustPressed(Keys.Escape))
                {
                    PopupEnvironment.Close();
                    _done = true;
                }
            }

            _outer.Update(position, width, height);
        }
    }
}
