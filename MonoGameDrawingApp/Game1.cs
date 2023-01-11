using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameDrawingApp.Ui;
using MonoGameDrawingApp.Ui.Tabs;
using MonoGameDrawingApp.Ui.TextInput;
using MonoGameDrawingApp.Ui.TextInput.Filters.Base;
using System;
using System.Diagnostics;

namespace MonoGameDrawingApp
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private IUiElement _split;
        private SpriteFont _font;
        private Random rnd = new Random();

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
            TargetElapsedTime = TimeSpan.FromSeconds(1d / 60d);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _font = Content.Load<SpriteFont>("font");
            TextInputField textInputField = new TextInputField(_font, "test", new ITextInputFilter[] { new AlphanumericTextInputFilter() }, true, true, -1);
            TabEnvironment env = new TabEnvironment(new CenterView(new MinSize(textInputField, 200, 40), true, true), _font);

            for (int i = 0; i < 100; i++)
            {
                env.TabBar.OpenTab(new BasicTab("Test" + i, new ColorRect(new Color(rnd.Next(256), rnd.Next(256), rnd.Next(256)))));
            }

            _split = env;
            /*
            ScrollBar scrollBar = new HScrollBar();
            scrollBar.Size = 100;
            scrollBar.End = 1000;
            scrollBar.ScrollSpeed = 0.05f;
            _split = new VSplitStandard(scrollBar, new ColorRect(Color.Green), 10);
            */
            //_split = new VSplitDraggable(new ColorRect(Color.Blue), new ColorRect(Color.Red), 100, 10);
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            _split.Update(Vector2.Zero, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            //_split.SplitPosition = Mouse.GetState().Y;
            // TODO: Add your drawing code here
            Graphics graphics = new Graphics(GraphicsDevice, _spriteBatch, Content);
            Texture2D render = _split.Render(graphics, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            GraphicsDevice.Clear(new Color(50, 50, 50));
            _spriteBatch.Begin();
            //Debug.WriteLine("Framerate: {0}", 1 / gameTime.ElapsedGameTime.TotalSeconds

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