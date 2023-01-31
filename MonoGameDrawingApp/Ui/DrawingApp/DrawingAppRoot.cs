using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameDrawingApp.Ui.Base;
using System;

namespace MonoGameDrawingApp.Ui.DrawingApp
{
    public class DrawingAppRoot : IUiElement
    {
        private UiEnvironment _environment;

        public DrawingAppRoot(UiEnvironment environment)
        {
            _environment = environment;
        }

        public bool Changed => throw new NotImplementedException();

        public int RequiredWidth => throw new NotImplementedException();

        public int RequiredHeight => throw new NotImplementedException();

        public UiEnvironment Environment => _environment;

        public Texture2D Render(Graphics graphics, int width, int height)
        {
            throw new NotImplementedException();
        }

        public void Update(Vector2 position, int width, int height)
        {
            throw new NotImplementedException();
        }
    }
}
