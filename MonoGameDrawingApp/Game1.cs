using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameDrawingApp.Ui;
using MonoGameDrawingApp.Ui.Base;
using MonoGameDrawingApp.Ui.Base.Split.Horizontal;
using MonoGameDrawingApp.Ui.FileSystemTree;
using MonoGameDrawingApp.Ui.FileSystemTree.MiscFileTypes.Png;
using MonoGameDrawingApp.Ui.Popup;
using MonoGameDrawingApp.Ui.Scroll;
using MonoGameDrawingApp.Ui.Tabs;
using MonoGameDrawingApp.Ui.Themes;
using MonoGameDrawingApp.Ui.Tree;
using MonoGameDrawingApp.Ui.Tree.Trees;
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

            ChangeableView treeHolder = new ChangeableView(environment, null); //weird fix, the tree needs the popupenvironemt, and the popupenvironment needs the tree

            TabEnvironment tabEnv = new TabEnvironment(environment,

                new HSplitDraggable(environment,
                    new StackView(environment, new IUiElement[]
                    {
                        new ColorRect(environment, environment.Theme.MenuBackgorundColor),

                        treeHolder

                    }),
                    new ColorRect(environment, Color.Transparent),
                    200,
                    10
                )

            );

            PopupEnvironment pop = new PopupEnvironment(environment, tabEnv);

            FileIcon[] fileIcons = new FileIcon[]
            {
                new FileIcon("txt", "icons/textfile"),
                new FileIcon("png", "icons/imagefile"),
            };

            IOpenableFileType[] fileTypes = new IOpenableFileType[]
            {
                new PngOpenableFileType()
            };

            FileSystemTree tree = new FileSystemTree(@"", pop, new FileTypeManager(tabEnv, fileTypes, fileIcons, true), true, true);

            treeHolder.Child = new ScrollWindow(environment, new TreeView(environment, 20, 3, tree, true));



            /*
            pop.OpenCentered(new TextInputPopup(
                environment: environment,
                popupEnvironment: pop,
                confirmed: (string s) => Debug.WriteLine("Seleted: " + s),
                filters: new ITextInputFilter[] { new AlphanumericTextInputFilter() },
                title: "Test Window",
                currentValue: "RenameMe"
            ));
            */

            for (int i = 0; i < 10; i++)
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
            text = new TextView(environment, "0");
            environment.Root = new StackView(environment, new IUiElement[]
            {
                pop,
                text
            });
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