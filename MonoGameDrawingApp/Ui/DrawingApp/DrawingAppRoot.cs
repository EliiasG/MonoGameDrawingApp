using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameDrawingApp.Ui.Base;
using MonoGameDrawingApp.Ui.Popup;
using MonoGameDrawingApp.Ui.Tabs;

namespace MonoGameDrawingApp.Ui.DrawingApp
{
    public class DrawingAppRoot : IUiElement
    {
        public readonly TabEnvironment TabEnvironment;
        public readonly PopupEnvironment PopupEnvironment;

        private UiEnvironment _environment;

        public DrawingAppRoot(UiEnvironment environment)
        {
            _environment = environment;
            TabEnvironment = new TabEnvironment(Environment, new DrawingAppStart(Environment, PopupEnvironment, TabEnvironment));
            PopupEnvironment = new PopupEnvironment(Environment, TabEnvironment);
        }

        public bool Changed => PopupEnvironment.Changed;

        public int RequiredWidth => PopupEnvironment.RequiredWidth;

        public int RequiredHeight => PopupEnvironment.RequiredHeight;

        public UiEnvironment Environment => _environment;

        public Texture2D Render(Graphics graphics, int width, int height)
        {
            return PopupEnvironment.Render(graphics, width, height);
        }

        public void Update(Vector2 position, int width, int height)
        {
            PopupEnvironment.Update(position, width, height);
        }
    }
}
