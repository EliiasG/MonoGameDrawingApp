using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameDrawingApp.Ui.Base.Buttons;
using MonoGameDrawingApp.Ui.Base.Lists;
using System.Collections.Generic;

namespace MonoGameDrawingApp.Ui.Base.Popup.ContextMenus
{
    public class ContextMenu : IUiElement
    {
        private MouseState _oldMouse;

        private readonly StackView _outer;
        private readonly IUiElement _inner;
        private readonly Button _button;

        public ContextMenu(UiEnvironment environment, IEnumerable<IUiElement> items, PopupEnvironment popupEnvironment)
        {
            Environment = environment;
            PopupEnvironment = popupEnvironment;


            _inner = new VListView<IUiElement>(environment, new List<IUiElement>(items));

            _button = new Button(environment, new CenterView(environment, _inner, true, true));

            _outer = new StackView(environment, new IUiElement[]
                {
                    new ColorRect(environment, environment.Theme.SecondaryMenuBackgroundColor),
                    _button,
                }
            );

            _oldMouse = Mouse.GetState();
        }

        public bool Changed => _outer.Changed;

        public int RequiredWidth => _inner.RequiredWidth + 8;

        public int RequiredHeight => _inner.RequiredHeight + 8;

        public UiEnvironment Environment { get; }

        public PopupEnvironment PopupEnvironment { get; }

        public Texture2D Render(Graphics graphics, int width, int height)
        {
            return _outer.Render(graphics, width, height);
        }

        public void Update(Vector2 position, int width, int height)
        {
            MouseState mouse = Mouse.GetState();
            bool justPressedLeft = mouse.LeftButton == ButtonState.Pressed && _oldMouse.LeftButton != ButtonState.Pressed;
            bool justPressedRight = mouse.RightButton == ButtonState.Pressed && _oldMouse.RightButton != ButtonState.Pressed;
            if (!_button.ContainsMouse && (justPressedLeft || justPressedRight))
            {
                PopupEnvironment.Close();
            }
            _outer.Update(position, width, height);
            _oldMouse = mouse;
        }
    }
}
