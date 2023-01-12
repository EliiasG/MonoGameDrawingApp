using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace MonoGameDrawingApp.Ui.Popup
{
    public class PopupEnvironment : IUiElement
    {
        public IUiElement Child;

        private readonly UpdateBlocker _updateBlocker;
        private readonly StackView _outer;
        private readonly PeekView _peekView;
        private readonly ChangeableView _popup;
        private readonly IUiElement _empty;
        private readonly UiEnvironment _environment;

        public PopupEnvironment(UiEnvironment environment, IUiElement child)
        {
            Child = child;

            _environment = environment;

            _empty = new ColorRect(environment, Color.Transparent);
            _popup = new ChangeableView(environment, _empty);
            _peekView = new PeekView(environment, _popup);
            _updateBlocker = new UpdateBlocker(environment, child);
            _outer = new StackView(environment, new List<IUiElement> { _updateBlocker, _peekView });
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
            _outer.Update(position, width, height);
            _updateBlocker.ShouldUpdate = _popup.Child == _empty;
        }

        public void Open(Point position, IUiElement popup)
        {
            _popup.Child = new StaticSize(Environment, popup);
            _peekView.Position = -position.ToVector2();
        }

        public void OpenCentered(IUiElement popup)
        {
            _popup.Child = new CenterView(Environment, popup, true, true);
            _peekView.Position = Vector2.Zero;
        }

        public void Close()
        {
            _popup.Child = _empty;
        }
    }
}
