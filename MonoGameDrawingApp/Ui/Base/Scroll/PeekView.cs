using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MonoGameDrawingApp.Ui.Base.Scroll
{
    public class PeekView : IScrollableView
    {
        private Vector2 _position = Vector2.Zero;
        private readonly RenderHelper _renderHelper;

        public PeekView(UiEnvironment environment, IUiElement child)
        {
            Environment = environment;

            _renderHelper = new RenderHelper();
            Child = child;
        }

        public int RequiredWidth => 1;

        public int RequiredHeight => 1;

        public bool Changed { get; private set; } = true;

        public UiEnvironment Environment { get; }

        public Vector2 Position
        {
            get => _position;
            set
            {
                if (_position != value)
                {
                    _position = value;
                    Changed = true;
                }
            }
        }

        public int Width { get; private set; } = 1;

        public int Height { get; private set; } = 1;

        public int MaxWidth => Child.RequiredWidth;

        public int MaxHeight => Child.RequiredHeight;

        public IUiElement Child { get; }

        public Texture2D Render(Graphics graphics, int width, int height)
        {
            _renderHelper.Begin(graphics, width, height);

            if (Changed || _renderHelper.SizeChanged)
            {
                Texture2D childRender = Child.Render(graphics, Math.Max(width, Child.RequiredWidth), Math.Max(height, Child.RequiredHeight));

                _renderHelper.BeginSpriteBatchDraw();

                graphics.SpriteBatch.Draw(
                    texture: childRender,
                    position: -Position,
                    color: Color.White
                );

                _renderHelper.FinishSpriteBatchDraw();
            }

            Changed = false;
            return _renderHelper.Result;
        }

        public void Update(Vector2 position, int width, int height)
        {
            Width = width;
            Height = height;
            Child.Update(position - Position, width, height);
            if (Child.Changed)
            {
                Changed = true;
            }
        }
    }
}
