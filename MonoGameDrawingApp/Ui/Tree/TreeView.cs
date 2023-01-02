using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameDrawingApp.Ui.Lists;
using MonoGameDrawingApp.Ui.Tree.TreeItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
