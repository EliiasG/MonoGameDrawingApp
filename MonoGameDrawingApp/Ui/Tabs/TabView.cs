using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameDrawingApp.Ui.Tabs
{
    public class TabView : IUiElement
    {
        public readonly Tab Tab;
        public readonly SpriteFont Font;
        public IUiElement Background;
        public IUiElement BackgroundSelected;
        public IUiElement CloseButton;

        public int Spacing;
        public int ExtraSize;

        private readonly RenderHelper _renderHelper;
        private MouseState _oldMouse;

        public TabView(Tab tab, SpriteFont font, int spacing = 2, int extraSize = 2)
        {
            Tab = tab;
            Font = font;
            Spacing = spacing;
            Background = new ColorRect(Color.Gray);
            BackgroundSelected = new ColorRect(Color.LightGray);
            CloseButton = new MinSize(new ScaleView(new SpriteView("icons/close",new Color(50, 50, 50))), 21, 21);
            //CloseButton = new TextView(Font, "X");
            ExtraSize = extraSize;

            _renderHelper = new RenderHelper();
        }

        public int RequiredWidth => (int)Font.MeasureString(Tab.Title).X + ExtraSize * 2 + Spacing + (Tab.HasCloseButton ? CloseButton.RequiredWidth : 0);

        public int RequiredHeight => Math.Max((int)Font.MeasureString(Tab.Title).Y, Tab.HasCloseButton ? CloseButton.RequiredHeight : 0) + ExtraSize;

        public Texture2D Render(Graphics graphics, Vector2 position, int width, int height)
        {
            
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

            IUiElement currentBackground = Tab.IsSelected ? BackgroundSelected : Background;
            Texture2D backgroundRender = currentBackground.Render(graphics, position, backgroundBounds.Width, height);
            Texture2D closeButtonRender = CloseButton.Render(graphics, position + closeButtonPosition, closeButtonBounds.Width, closeButtonBounds.Height);

            _renderHelper.Begin(graphics, width, height);
            
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
            
            _oldMouse = mouse;
            
            return _renderHelper.FinishDraw();
        }
    }
}
