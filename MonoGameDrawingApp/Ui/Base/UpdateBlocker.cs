using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameDrawingApp.Ui.Base
{
    public class UpdateBlocker : IUiElement
    {
        public readonly IUiElement Child;

        private readonly UiEnvironment _environment;

        public bool ShouldUpdate { get; set; }

        public UpdateBlocker(UiEnvironment environment, IUiElement child)
        {
            _environment = environment;

            Child = child;
            ShouldUpdate = true;
        }

        public bool Changed => Child.Changed;

        public int RequiredWidth => Child.RequiredWidth;

        public int RequiredHeight => Child.RequiredHeight;

        public UiEnvironment Environment => _environment;

        public Texture2D Render(Graphics graphics, int width, int height)
        {
            return Child.Render(graphics, width, height);
        }

        public void Update(Vector2 position, int width, int height)
        {
            if (ShouldUpdate)
            {
                Child.Update(position, width, height);
            }
        }
    }
}
