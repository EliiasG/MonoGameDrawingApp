using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameDrawingApp.Ui.Base.Lists;
using System.Collections.Generic;

namespace MonoGameDrawingApp.Ui.Base.Popup.ContextMenus.Items
{
    public class ContextMenuSeperator : IUiElement
    {
        private readonly UiEnvironment _environment;

        private readonly IUiElement _child;

        public ContextMenuSeperator(UiEnvironment environment)
        {
            _environment = environment;
            _child = new VListView<IUiElement>(environment, new List<IUiElement>
            {
                new EmptySpace(Environment,  0, 5),
                new MinSize(environment, new ColorRect(environment, environment.Theme.SelectedButtonColor), 0, 2),
                new EmptySpace(Environment,  0, 5),
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
