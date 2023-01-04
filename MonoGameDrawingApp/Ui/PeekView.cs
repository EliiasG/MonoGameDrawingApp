using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MonoGameDrawingApp.Ui
{
    public class PeekView : IUiElement
    {
        public readonly IUiElement Child;

        private Vector2 _position = Vector2.Zero;
        private RenderHelper _renderHelper;
        private bool _changed = true;

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

        public PeekView(IUiElement child)
        {
            _renderHelper = new RenderHelper();
            Child = child;
        }

        public int RequiredWidth => 1;

        public int RequiredHeight => 1;

        public bool Changed => _changed;

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
            Child.Update(position - Position, width, height);
            if (Child.Changed)
            {
                _changed = true;
            }
        }
    }
}
