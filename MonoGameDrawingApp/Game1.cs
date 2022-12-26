using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameDrawingApp.Ui;
using MonoGameDrawingApp.Ui.Scroll;
using MonoGameDrawingApp.Ui.Split.Horizontal;
using MonoGameDrawingApp.Ui.Split.Vertical;
using MonoGameDrawingApp.Ui.Tabs;
using System.Diagnostics;

namespace MonoGameDrawingApp
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private IUiElement _split;
        private SpriteFont _font;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Window.AllowAltF4 = true;
            Window.AllowUserResizing = true;
        }

        protected override void Initialize()
        {
            Debug.WriteLine("Started!");
            // TODO: Add your initialization logic here


            //_split = new ColorRect(Color.Gold);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _font = Content.Load<SpriteFont>("font");

            TabBar top = new TabBar(_font);
            top.OpenTab(new BasicTab("test"));
            top.OpenTab(new BasicTab("test2"));
            _split = new VSplitDraggable(top,new ScaleView(new TextView(_font, "Test Name X")), 0, 10);
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            //_split.SplitPosition = Mouse.GetState().Y;
            // TODO: Add your drawing code here
            Graphics graphics = new Graphics(GraphicsDevice, _spriteBatch);
            Texture2D render = _split.Render(graphics, Vector2.Zero, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            Mouse.SetCursor(graphics.Cursor);
            _spriteBatch.Begin();
            _spriteBatch.Draw(
                texture: render,
                position: new Vector2(0),
                color: Color.White
            );
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}