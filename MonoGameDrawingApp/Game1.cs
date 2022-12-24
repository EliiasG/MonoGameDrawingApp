using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameDrawingApp.Ui;
using MonoGameDrawingApp.Ui.Scroll;
using MonoGameDrawingApp.Ui.Split.Horizontal;
using MonoGameDrawingApp.Ui.Split.Vertical;
using System.Diagnostics;

namespace MonoGameDrawingApp
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private IUiElement _split;

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
            
            //crap code, just for testing
            IUiElement test = new VSplitDraggable(new HSplitDraggable(new ColorRect(Color.Green), new ColorRect(Color.Blue), 200, 10), new ColorRect(Color.Brown), 500, 10);
            test = new MinSize(test, 1000, 1000);

            ScrollWindow scrollBar = new ScrollWindow(test, true, true);

            scrollBar.HScrollBar.ScrollSpeed = 0.05f;
            scrollBar.VScrollBar.ScrollSpeed = 0.05f;

            IUiElement top = new HSplitStandard(scrollBar, new ColorRect(Color.Gold), 500);
            _split = new VSplitStandard(top, new ColorRect(Color.Red), 500);
            //_split = new ColorRect(Color.Gold);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

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