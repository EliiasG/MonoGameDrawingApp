using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameDrawingApp.Ui.Lists;
using System.Collections.Generic;

namespace MonoGameDrawingApp.Ui.Popup.ContextMenu.Items
{
    public class ContextMenuSeperator : IUiElement
    {
        private readonly UiEnvironment _environment;

        private IUiElement _child;

        public ContextMenuSeperator(UiEnvironment environment)
        {
            _environment = environment;
            _child = new VListView<IUiElement>(environment, new List<IUiElement>
            {
                new MinSize(environment, new ColorRect(environment, Color.Transparent), 0, 10),
                new MinSize(environment, new ColorRect(environment, environment.Theme.ButtonColor), 0, 10),
                new MinSize(environment, new ColorRect(environment, Color.Transparent), 0, 10),
            });
        }

        public bool Changed => _child.Changed;

        public int RequiredWidth => _child.RequiredWidth;

        public int RequiredHeight => _child.RequiredHeight;

        public UiEnvironment Environment => _environment;

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
