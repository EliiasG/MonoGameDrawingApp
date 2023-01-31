using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameDrawingApp.Ui.Base;
using System;

namespace MonoGameDrawingApp.Ui.Tabs
{
    public class TabView : IUiElement
    {
        // class is a big mess, should use exisiting elements instead of rendering by itself
        public readonly Tab Tab;

        public readonly int Spacing;
        public readonly int ExtraSize;

        private readonly UiEnvironment _environment;

        private readonly IUiElement _background;
        private readonly IUiElement _BackgroundSelected;
        private readonly IUiElement _closeButton;

        private readonly RenderHelper _renderHelper;
        private MouseState _oldMouse;
        private bool _changed;

        private string _oldTile = "";
        private bool _oldSelected = false;

        public TabView(UiEnvironment environment, Tab tab, int spacing = 2, int extraSize = 2)
        {
            _environment = environment;

            Tab = tab;
            Spacing = spacing;
            int size = (int)Environment.FontHeight;
            _background = new ColorRect(environment, environment.Theme.ButtonColor);
            _BackgroundSelected = new ColorRect(environment, environment.Theme.SelectedButtonColor);
            _closeButton = new MinSize(environment, new ScaleView(environment, new SpriteView(environment, "icons/close")), size, size);
            ExtraSize = extraSize;

            _renderHelper = new RenderHelper();
        }

        public int RequiredWidth => (int)Environment.Font.MeasureString(Tab.Title).X + ExtraSize + Spacing + (Tab.HasCloseButton ? _closeButton.RequiredWidth : 0);

        public int RequiredHeight => Math.Max((int)Environment.Font.MeasureString(Tab.Title).Y, Tab.HasCloseButton ? _closeButton.RequiredHeight : 0) + ExtraSize;

        public bool Changed => _changed;

        public UiEnvironment Environment => _environment;

        public void Update(Vector2 position, int width, int height)
        {
            _updateChanged();
            MouseState mouse = Mouse.GetState();

            Rectangle backgroundBounds = new Rectangle(position.ToPoint(), new Point(width - Spacing, height));

            bool isIn = backgroundBounds.Contains(mouse.Position);
            bool justPressed = mouse.LeftButton == ButtonState.Pressed && _oldMouse.LeftButton == ButtonState.Released;

            Vector2 closeButtonPosition = new Vector2(width - _closeButton.RequiredWidth - ExtraSize - Spacing, ExtraSize);
            Rectangle closeButtonBounds = new Rectangle((closeButtonPosition + position).ToPoint(), new Point(_closeButton.RequiredWidth, _closeButton.RequiredHeight));

            bool isInCloseButton = closeButtonBounds.Contains(mouse.Position);


            if (justPressed && isInCloseButton && Tab.HasCloseButton)
            {
                Tab.Close();
            }

            else if (isIn && justPressed)
            {
                Tab.TabBar.SelectedTab = Tab;
            }

            _oldMouse = mouse;
        }

        private void _updateChanged()
        {
            if (_oldTile != Tab.Title || _oldSelected != Tab.IsSelected || _closeButton.Changed)
            {
                _changed = true;
            }

            _oldSelected = Tab.IsSelected;
            _oldTile = Tab.Title;
        }

        public Texture2D Render(Graphics graphics, int width, int height)
        {
            _renderHelper.Begin(graphics, width, height);

            if (Changed || _renderHelper.SizeChanged)
            {
                Vector2 closeButtonPosition = new Vector2(width - _closeButton.RequiredWidth - ExtraSize, ExtraSize);

                IUiElement currentBackground = Tab.IsSelected ? _BackgroundSelected : _background;
                Texture2D backgroundRender = currentBackground.Render(graphics, width, height);
                Texture2D closeButtonRender = _closeButton.Render(graphics, _closeButton.RequiredWidth, _closeButton.RequiredHeight);

                _renderHelper.BeginDraw();

                graphics.SpriteBatch.Draw(
                    texture: backgroundRender,
                    position: Vector2.Zero,
                    color: Color.White
                );

                if (Tab.HasCloseButton)
                {
                    graphics.SpriteBatch.Draw(
                        texture: closeButtonRender,
                        position: closeButtonPosition,
                        color: Environment.Theme.DefaultTextColor
                    );
                }

                graphics.SpriteBatch.DrawString(
                    spriteFont: Environment.Font,
                    text: Tab.Title,
                    position: new Vector2(ExtraSize, (height - (int)Environment.Font.MeasureString(Tab.Title).Y) / 2 + 1),
                    color: Tab.IsSelected ? Environment.Theme.HoveringTextColor : Environment.Theme.DefaultTextColor
                );

                _renderHelper.FinishDraw();
            }

            _changed = false;
            return _renderHelper.Result;
        }
    }
}
