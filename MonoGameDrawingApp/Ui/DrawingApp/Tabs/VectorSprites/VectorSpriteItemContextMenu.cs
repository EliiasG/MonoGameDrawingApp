using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameDrawingApp.Ui.Base;
using MonoGameDrawingApp.Ui.Base.Popup;
using MonoGameDrawingApp.VectorSprites;

namespace MonoGameDrawingApp.Ui.DrawingApp.Tabs.VectorSprites
{
    public class VectorSpriteItemContextMenu : IUiElement
    {
        private readonly VectorSpriteItem _item;
        private readonly PopupEnvironment _popupEnvironment;
        private readonly IUiElement _root;

        public VectorSpriteItemContextMenu(UiEnvironment environment, VectorSpriteItem item, PopupEnvironment popupEnvironment)
        {
            Environment = environment;
            _item = item;
            _popupEnvironment = popupEnvironment;


        }

        public bool Changed => _root.Changed;

        public int RequiredWidth => _root.RequiredWidth;

        public int RequiredHeight => _root.RequiredHeight;

        public UiEnvironment Environment { get; init; }

        public Texture2D Render(Graphics graphics, int width, int height)
        {
            return _root.Render(graphics, width, height);
        }

        public void Update(Vector2 position, int width, int height)
        {
            _root.Update(position, width, height);
        }
    }
}
