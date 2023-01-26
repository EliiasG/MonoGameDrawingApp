using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameDrawingApp.Ui.Buttons;
using MonoGameDrawingApp.Ui.List;
using System.Collections.Generic;

namespace MonoGameDrawingApp.Ui.Popup
{
    public class MessagePopup : IUiElement
    {
        public readonly string Message;

        private const int Spacing = 5;

        private readonly UiEnvironment _environment;
        private readonly PopupEnvironment _popupEnvironment;

        private readonly IUiElement _outer;
        private readonly TextButton _close;

        public MessagePopup(UiEnvironment environment, string message, PopupEnvironment popupEnvironment)
        {
            Message = message;
            _environment = environment;
            _popupEnvironment = popupEnvironment;

            _close = new TextButton(environment, "Close");

            _outer = new StackView(environment, new List<IUiElement>()
            {
                new ColorRect(environment, environment.Theme.SecondaryMenuBackgroundColor),
                new VListView<IUiElement>(environment, new List<IUiElement>
                {
                    new MinSize(environment, new ColorRect(environment, Color.Transparent), 1, Spacing),
                    new HListView<IUiElement>(Environment, new List<IUiElement>
                    {
                        new MinSize(Environment, new ColorRect(environment, Color.Transparent), Spacing, 1),
                        new ColorModifier(environment, new TextView(environment, message), environment.Theme.EditingTextColor),
                        new MinSize(Environment, new ColorRect(environment, Color.Transparent), Spacing, 1),
                    }),
                    new MinSize(environment, new ColorRect(environment, Color.Transparent), 1, Spacing * 2),
                    new CenterView(environment, _close, true, false),
                    new MinSize(environment, new ColorRect(environment, Color.Transparent), 1, Spacing),
                }),
            });
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
            if (_close.Button.JustLeftClicked)
            {
                _popupEnvironment.Close();
            }

            _outer.Update(position, width, height);
        }
    }
}
