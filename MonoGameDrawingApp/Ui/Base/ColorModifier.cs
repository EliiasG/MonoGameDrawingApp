using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameDrawingApp.Ui.Base
{
    public class ColorModifier : IUiElement
    {
        private Color _color;

        private readonly RenderHelper _renderHelper;

        public ColorModifier(UiEnvironment environment, IUiElement child, Color color)
        {
            Environment = environment;

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
                    Changed = true;
                    _color = value;
                }
            }
            get => _color;
        }

        public bool Changed { get; private set; }

        public int RequiredWidth => Child.RequiredWidth;

        public int RequiredHeight => Child.RequiredHeight;

        public UiEnvironment Environment { get; }

        public IUiElement Child { get; }

        public UiEnvironment Environment1 => Environment;

        public Texture2D Render(Graphics graphics, int width, int height)
        {
            _renderHelper.Begin(graphics, width, height);

            if (Changed || _renderHelper.SizeChanged)
            {
                Changed = false;
                Texture2D render = Child.Render(graphics, width, height);

                _renderHelper.BeginSpriteBatchDraw();

                graphics.SpriteBatch.Draw(
                    texture: render,
                    position: Vector2.Zero,
                    color: Color
                );

                _renderHelper.FinishSpriteBatchDraw();
            }

            return _renderHelper.Result;
        }

        public void Update(Vector2 position, int width, int height)
        {
            Child.Update(position, width, height);
            Changed = Changed || Child.Changed;
        }
    }
}
