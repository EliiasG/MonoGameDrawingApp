using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameDrawingApp.Ui;
using MonoGameDrawingApp.Ui.Base;
using MonoGameDrawingApp.Ui.DrawingApp;
using MonoGameDrawingApp.Ui.Themes;
using System;
using System.Diagnostics;

namespace MonoGameDrawingApp
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private UiEnvironment environment;
        private Random rnd = new Random();
        private TextView text;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Window.AllowAltF4 = true;
            Window.AllowUserResizing = true;
            _graphics.PreferredBackBufferWidth = 1600;
            _graphics.PreferredBackBufferHeight = 900;
        }

        protected override void Initialize()
        {
            Debug.WriteLine("Started!");
            // TODO: Add your initialization logic here


            //_split = new ColorRect(Color.Gold);
            TargetElapsedTime = TimeSpan.FromSeconds(1d / 60d);
            //IOHelper.OpenInExplorer(@"C:\Projects\TestFolder\DoesNotExist\Test.txt

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            Graphics graphics = new Graphics(GraphicsDevice, _spriteBatch);
            environment = new UiEnvironment(graphics, new DarkTheme(), Content.Load<SpriteFont>("font"), Content);

            // TODO: use this.Content to load your game content here
            text = new TextView(environment, "0");
            environment.Root = new StackView(environment, new IUiElement[]
            {
                new DrawingAppRoot(environment),
                text,
            }); ;
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            //_split.SplitPosition = Mouse.GetState().Y;
            // TODO: Add your drawing code here
            text.Text = "" + 1 / (gameTime.ElapsedGameTime.TotalSeconds);
            environment.Render();
            base.Draw(gameTime);
        }
    }
}