using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameDrawingApp.Ui
{
    public class ChangeableView : IUiElement
    {
        private IUiElement _child;

        private bool _changed;

        public ChangeableView(IUiElement child)
        {
            _child = child;
        }

        public IUiElement Child 
        { 
            get 
            { 
                return _child; 
            }
            set
            {
                if (_child != value && value != null)
                {
                    _child = value;
                    _changed = true;
                }
            }
        }
        public bool Changed => _changed;

        public int RequiredWidth => Child.RequiredWidth;

        public int RequiredHeight => Child.RequiredHeight;

        public Texture2D Render(Graphics graphics, int width, int height)
        {
            _changed = false;
            return Child.Render(graphics, width, height);
        }

        public void Update(Vector2 position, int width, int height)
        {
            Child.Update(position, width, height);
            if(Child.Changed)
            {
                _changed = true;
            }
        }
    }
}
