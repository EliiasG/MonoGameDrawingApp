using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MonoGameDrawingApp.Ui.Scroll
{
    public class PeekView : IScrollableView
    {
        public readonly IUiElement Child;

        private readonly UiEnvironment _environment;

        private Vector2 _position = Vector2.Zero;
        private RenderHelper _renderHelper;
        private bool _changed = true;

        private int _width = 1;
        private int _height = 1;
        public PeekView(UiEnvironment environment, IUiElement child)
        {
            _environment = environment;

            _renderHelper = new RenderHelper();
            Child = child;
        }

        public int RequiredWidth => 1;

        public int RequiredHeight => 1;

        public bool Changed => _changed;

        public UiEnvironment Environment => _environment;

        public Vector2 Position
        {
            get => _position;
            set
            {
                if (_position != value)
                {
                    _position = value;
                    _changed = true;
                }
            }
        }

        public int Width => _width;

        public int Height => _height;

        public int MaxWidth => Child.RequiredWidth;

        public int MaxHeight => Child.RequiredHeight;

        public Texture2D Render(Graphics graphics, int width, int height)
        {
            _renderHelper.Begin(graphics, width, height);

            if (_changed || _renderHelper.SizeChanged)
            {
                Texture2D childRender = Child.Render(graphics, Math.Max(width, Child.RequiredWidth), Math.Max(height, Child.RequiredHeight));

                _renderHelper.BeginDraw();

                graphics.SpriteBatch.Draw(
                    texture: childRender,
                    position: -Position,
                    color: Color.White
                );

                _renderHelper.FinishDraw();
            }

            _changed = false;
            return _renderHelper.Result;
        }

        public void Update(Vector2 position, int width, int height)
        {
            _width = width;
            _height = height;
            Child.Update(position - Position, width, height);
            if (Child.Changed)
            {
                _changed = true;
            }
        }
    }
}
