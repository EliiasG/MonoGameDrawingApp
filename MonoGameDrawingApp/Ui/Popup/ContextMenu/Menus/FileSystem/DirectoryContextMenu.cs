using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameDrawingApp.Ui.Popup.ContextMenu.Menus.FileSystem
{
    public class DirectoryContextMenu : IUiElement
    {
        public readonly string Path;

        private UiEnvironment _environment;

        private StackView _outer;

        public DirectoryContextMenu(UiEnvironment environment, string path)
        {
            Path = path;
            _environment = environment;
            _outer = new StackView(
                environment: environment,
                children: new IUiElement[]
                {
                    new ColorRect(environment, environment.Theme.SecondaryMenuBackgroundColor),
                    new TextView(environment, "WIP")//TODO
                }
            );
        }

        public bool Changed => _outer.Changed;

        public int RequiredWidth => _outer.RequiredWidth;

        public int RequiredHeight => _outer.RequiredHeight;

        public UiEnvironment Environment => _environment;

        public Texture2D Render(Graphics graphics, int width, int height)
        {
            return _outer.Render(graphics, width, height);
        }

        public void Update(Vector2 position, int width, int height)
        {
            _outer.Update(position, width, height);
        }
    }
}
