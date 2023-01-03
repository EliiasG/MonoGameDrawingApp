﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace MonoGameDrawingApp.Ui.Tabs
{
    public class TabView : IUiElement
    {
        public readonly Tab Tab;
        public readonly SpriteFont Font;
        public readonly IUiElement Background;
        public readonly IUiElement BackgroundSelected;
        public readonly IUiElement CloseButton;

        public readonly int Spacing;
        public readonly int ExtraSize;

        private readonly RenderHelper _renderHelper;
        private MouseState _oldMouse;
        private bool _changed;

        private string oldTile = "";
        private bool oldSelected = false;

        public TabView(Tab tab, SpriteFont font, int spacing = 2, int extraSize = 2)
        {
            Tab = tab;
            Font = font;
            Spacing = spacing;
            Background = new ColorRect(Color.Gray);
            BackgroundSelected = new ColorRect(Color.LightGray);
            CloseButton = new MinSize(new ScaleView(new SpriteView("icons/close", new Color(50, 50, 50))), 21, 21);
            //CloseButton = new TextView(Font, "X");
            ExtraSize = extraSize;

            _renderHelper = new RenderHelper();
        }

        public int RequiredWidth => (int)Font.MeasureString(Tab.Title).X + ExtraSize * 2 + Spacing + (Tab.HasCloseButton ? CloseButton.RequiredWidth : 0);

        public int RequiredHeight => Math.Max((int)Font.MeasureString(Tab.Title).Y, Tab.HasCloseButton ? CloseButton.RequiredHeight : 0) + ExtraSize;

        public bool Changed => _changed;

        public void Update(Vector2 position, int width, int height)
        {
            _updateChanged();
            MouseState mouse = Mouse.GetState();

            Rectangle backgroundBounds = new Rectangle(position.ToPoint(), new Point(width - Spacing, height));

            bool isIn = backgroundBounds.Contains(mouse.Position);
            bool justPressed = mouse.LeftButton == ButtonState.Pressed && _oldMouse.LeftButton == ButtonState.Released;

            Vector2 closeButtonPosition = new Vector2(width - CloseButton.RequiredWidth - ExtraSize - Spacing, ExtraSize);
            Rectangle closeButtonBounds = new Rectangle((closeButtonPosition + position).ToPoint(), new Point(CloseButton.RequiredWidth, CloseButton.RequiredHeight));

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
            if (oldTile != Tab.Title)
            {
                _changed = true;
            }
            if (oldSelected != Tab.IsSelected)
            {
                _changed = true;
            }

            oldSelected = Tab.IsSelected;
            oldTile = Tab.Title;
        }

        public Texture2D Render(Graphics graphics, int width, int height)
        {
            _renderHelper.Begin(graphics, width, height);

            if (Changed || _renderHelper.SizeChanged)
            {
                Vector2 closeButtonPosition = new Vector2(width - CloseButton.RequiredWidth - ExtraSize - Spacing, ExtraSize);
                IUiElement currentBackground = Tab.IsSelected ? BackgroundSelected : Background;
                Texture2D backgroundRender = currentBackground.Render(graphics, width - Spacing, height);
                Texture2D closeButtonRender = CloseButton.Render(graphics, CloseButton.RequiredWidth, CloseButton.RequiredHeight);

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
                        color: Color.White
                    );
                }

                graphics.SpriteBatch.DrawString(
                    spriteFont: Font,
                    text: Tab.Title,
                    position: new Vector2(ExtraSize, 0),
                    color: Tab.IsSelected ? new Color(100, 100, 100) : Color.LightGray
                );

                _renderHelper.FinishDraw();
            }

            _changed = false;
            return _renderHelper.Result;
        }
    }
}
