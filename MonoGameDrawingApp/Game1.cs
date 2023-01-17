﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameDrawingApp.Ui;
using MonoGameDrawingApp.Ui.Popup;
using MonoGameDrawingApp.Ui.Tabs;
using MonoGameDrawingApp.Ui.TextInput;
using MonoGameDrawingApp.Ui.TextInput.Filters.Base;
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
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            Graphics graphics = new Graphics(GraphicsDevice, _spriteBatch, Content);
            environment = new UiEnvironment(graphics, new DarkTheme(), Content.Load<SpriteFont>("font"));

            TextInputField textInputField = new TextInputField(environment, "test", new ITextInputFilter[] { new AlphanumericTextInputFilter() }, true, true, false, -1);
            

            TabEnvironment tabEnv = new TabEnvironment(environment, new ColorRect(environment, Color.Transparent));

            PopupEnvironment pop = new PopupEnvironment(environment, tabEnv);

            pop.OpenCentered(new TextInputPopup(
                environment: environment,
                popupEnvironment: pop,
                confirmed: (string s) => Debug.WriteLine("Seleted: " + s),
                filters: new ITextInputFilter[] { new AlphanumericTextInputFilter() },
                title: "Test Window"
            ));

            for (int i = 0; i < 100; i++)
            {
                tabEnv.TabBar.OpenTab(new BasicTab("Test" + i, new ColorRect(environment, new Color(rnd.Next(256), rnd.Next(256), rnd.Next(256)))));
            }

            /*
            ScrollBar scrollBar = new HScrollBar();
            scrollBar.Size = 100;
            scrollBar.End = 1000;
            scrollBar.ScrollSpeed = 0.05f;
            _split = new VSplitStandard(scrollBar, new ColorRect(Color.Green), 10);
            */
            //_split = new VSplitDraggable(new ColorRect(Color.Blue), new ColorRect(Color.Red), 100, 10);
            // TODO: use this.Content to load your game content here
            environment.Root = pop;
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            //_split.SplitPosition = Mouse.GetState().Y;
            // TODO: Add your drawing code here
            environment.Render();
            base.Draw(gameTime);
        }
    }
}