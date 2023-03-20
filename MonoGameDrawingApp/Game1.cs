using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameDrawingApp.Ui.Base;
using MonoGameDrawingApp.Ui.Base.Themes;
using MonoGameDrawingApp.Ui.DrawingApp;
using System;
using System.Diagnostics;

namespace MonoGameDrawingApp
{
    public class Game1 : Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private UiEnvironment environment;
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

            //_split = new ColorRect(Color.Gold);
            TargetElapsedTime = TimeSpan.FromSeconds(1d / 60d);
            //IOHelper.OpenInExplorer(@"C:\Projects\TestFolder\DoesNotExist\Test.txt

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            Graphics graphics = new(GraphicsDevice, _spriteBatch, new TriangleBatch(GraphicsDevice));
            environment = new UiEnvironment(graphics, new DarkTheme(), Content.Load<SpriteFont>("font"), Content);

            environment.AddShortcut(new GlobalShortcut(new Keys[] { Keys.LeftControl, Keys.LeftShift, Keys.G }, GC.Collect));

            text = new TextView(environment, "");
            environment.Root = new StackView(environment, new IUiElement[]
            {
                new DrawingAppRoot(environment),
                //text,
            });
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            //_split.SplitPosition = Mouse.GetState().Y;
            DateTime before = DateTime.UtcNow;
            environment.Render();
            DateTime after = DateTime.UtcNow;
            text.Text = (1f / ((after.Ticks - before.Ticks) / 10_000_000f)).ToString();

            base.Draw(gameTime);
        }
    }
}