using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MonoGameDrawingApp.Ui.ContextMenu
{
    public class ContextMenuEnvironment : IUiElement
    {
        //TODO
        public bool Changed => throw new NotImplementedException();

        public int RequiredWidth => throw new NotImplementedException();

        public int RequiredHeight => throw new NotImplementedException();

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
