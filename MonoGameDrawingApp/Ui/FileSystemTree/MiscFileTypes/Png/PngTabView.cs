using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameDrawingApp.Ui.Base;

namespace MonoGameDrawingApp.Ui.FileSystemTree.MiscFileTypes.Png
{
    public class PngTabView : IUiElement
    {
        public string Path;

        private const int Spacing = 20;

        private UiEnvironment _environment;

        private IUiElement _root;

        private MinSize _minSize;

        public PngTabView(UiEnvironment environment, string path)
        {
            _environment = environment;
            Path = path;
            _minSize = new MinSize(
                environment: environment,
                child: new ScaleView(
                    environment: environment,
                    child: new ExternalImageView(
                        environment: environment,
                        path: path
                    )
                ),
                width: 1,
                height: 1
            );

            _root = new CenterView(environment, _minSize, true, true);
        }

        public bool Changed => _root.Changed;

        public int RequiredWidth => _root.RequiredWidth;

        public int RequiredHeight => _root.RequiredHeight;

        public UiEnvironment Environment => _environment;

        public Texture2D Render(Graphics graphics, int width, int height)
        {
            return _root.Render(graphics, width, height);
        }

        public void Update(Vector2 position, int width, int height)
        {
            _root.Update(position, width, height);
            int size = MathHelper.Min(width, height) - Spacing;
            _minSize.MinWidth = size;
            _minSize.MinHeight = size;
        }
    }
}
