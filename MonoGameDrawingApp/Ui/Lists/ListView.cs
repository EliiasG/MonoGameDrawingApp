using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameDrawingApp.Ui.Lists
{
    public abstract class ListView<T> : IUiElement where T : IUiElement
    {
        public List<T> Items;

        public ListView(List<T> items) 
        {
            Items = items;
        }

        public abstract int RequiredWidth { get; }

        public abstract int RequiredHeight { get; }

        public abstract Texture2D Render(Graphics graphics, Vector2 position, int width, int height);
    }
}
