using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameDrawingApp.Ui.Base;
using System;

namespace MonoGameDrawingApp.Ui.FileSystemTree.MiscFileTypes.Png
{
    public class ImageTabView : IUiElement
    {
        public string Path;

        private const int Spacing = 20;

        private UiEnvironment _environment;

        private IUiElement _root;

        private IUiElement _image;

        private MinSize _minSize;

        public ImageTabView(UiEnvironment environment, string path)
        {
            _environment = environment;
            Path = path;
            _image = new ExternalImageView(
                environment: environment,
                path: path
            );

            _minSize = new MinSize(
                environment: environment,
                child: new ScaleView(
                    environment: environment,
                    child: _image,
                    disableBlur: true
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
            int size = Math.Min(width, height) - Spacing;
            float ratio = _image.RequiredHeight / (float)_image.RequiredWidth;
            _minSize.MinWidth = ratio > 1 ? (int)(size / ratio) : size;
            _minSize.MinHeight = ratio < 1 ? (int)(size * ratio) : size;
        }
    }
}
