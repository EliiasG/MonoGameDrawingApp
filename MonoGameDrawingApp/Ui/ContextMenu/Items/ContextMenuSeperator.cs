using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameDrawingApp.Ui.Lists;
using System.Collections.Generic;

namespace MonoGameDrawingApp.Ui.ContextMenu.Items
{
    public class ContextMenuSeperator : IUiElement
    {
        private IUiElement _child;

        public ContextMenuSeperator()
        {
            _child = new VListView<IUiElement>(new List<IUiElement>
            {
                new MinSize(new ColorRect(Color.Transparent), 0, 10),
                new MinSize(new ColorRect(Color.DarkGray), 0, 10),
                new MinSize(new ColorRect(Color.Transparent), 0, 10),
            });
        }

        public bool Changed => _child.Changed;

        public int RequiredWidth => _child.RequiredWidth;

        public int RequiredHeight => _child.RequiredHeight;

        public Texture2D Render(Graphics graphics, int width, int height)
        {
            return _child.Render(graphics, width, height);
        }

        public void Update(Vector2 position, int width, int height)
        {
            _child.Update(position, width, height);
        }
    }
}
