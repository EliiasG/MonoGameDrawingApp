using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MonoGameDrawingApp.Ui.Tree
{
    public class TreeView : IUiElement
    {
        public TreeView() 
        {
        }

        public int RequiredWidth => throw new NotImplementedException();

        public int RequiredHeight => throw new NotImplementedException();

        public Texture2D Render(Graphics graphics, Vector2 position, int width, int height)
        {
            throw new NotImplementedException();
        }
    }
}
