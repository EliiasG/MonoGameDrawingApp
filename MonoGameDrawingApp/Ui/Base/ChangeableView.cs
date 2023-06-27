using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameDrawingApp.Ui.Base
{
    public class ChangeableView : IUiElement
    {
        private IUiElement _child;

        public ChangeableView(UiEnvironment environment, IUiElement child)
        {
            Environment = environment;
            _child = child;
        }

        public IUiElement Child
        {
            get => _child;
            set
            {
                if (_child != value && value != null)
                {
                    _child = value;
                    Changed = true;
                }
            }
        }
        public bool Changed { get; private set; }

        public int RequiredWidth => Child.RequiredWidth;

        public int RequiredHeight => Child.RequiredHeight;

        public UiEnvironment Environment { get; }

        public UiEnvironment Environment1 => Environment;

        public bool Changed1 { get => Changed; set => Changed = value; }

        public Texture2D Render(Graphics graphics, int width, int height)
        {
            Changed = false;
            return Child.Render(graphics, width, height);
        }

        public void Update(Vector2 position, int width, int height)
        {
            Child.Update(position, width, height);
            if (Child.Changed)
            {
                Changed = true;
            }
        }
    }
}
