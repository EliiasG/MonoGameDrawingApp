using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameDrawingApp.Ui
{
    public class ColorModifier : IUiElement
    {


        public readonly IUiElement Child;

        private Color _color;

        private readonly RenderHelper _renderHelper;

        private bool _changed;

        public ColorModifier(IUiElement child, Color color)
        {
            Color = color;
            Child = child;
            _renderHelper = new RenderHelper();
        }

        public Color Color
        {
            set
            {
                if (_color != value)
                {
                    _changed = true;
                    _color = value;
                }
            }
            get => _color;
        }

        public bool Changed => _changed;

        public int RequiredWidth => Child.RequiredWidth;

        public int RequiredHeight => Child.RequiredHeight;

        public Texture2D Render(Graphics graphics, int width, int height)
        {
            _renderHelper.Begin(graphics, width, height);

            if (_changed || _renderHelper.SizeChanged)
            {
                _changed = false;
                Texture2D render = Child.Render(graphics, width, height);

                _renderHelper.BeginDraw();

                graphics.SpriteBatch.Draw(
                    texture: render,
                    position: Vector2.Zero,
                    color: Color
                );

                _renderHelper.FinishDraw();
            }

            return _renderHelper.Result;
        }

        public void Update(Vector2 position, int width, int height)
        {
            Child.Update(position, width, height);
            _changed = _changed || Child.Changed;
        }
    }
}
