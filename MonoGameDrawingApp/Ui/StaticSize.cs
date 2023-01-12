using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameDrawingApp.Ui
{
    public class StaticSize : IUiElement
    {
        public readonly IUiElement Child;

        private readonly UiEnvironment _environment;

        private RenderHelper _renderHelper;

        public StaticSize(UiEnvironment environment, IUiElement child)
        {
            _environment = environment;

            Child = child;
            _renderHelper = new RenderHelper();
        }

        public bool Changed => Child.Changed;

        public int RequiredWidth => Child.RequiredWidth;

        public int RequiredHeight => Child.RequiredHeight;

        public UiEnvironment Environment => _environment;

        public Texture2D Render(Graphics graphics, int width, int height)
        {
            _renderHelper.Begin(graphics, width, height);

            if (_renderHelper.SizeChanged || Changed)
            {
                Texture2D render = Child.Render(graphics, Child.RequiredWidth, Child.RequiredWidth);

                _renderHelper.BeginDraw();

                graphics.SpriteBatch.Draw(
                    texture: render,
                    position: Vector2.Zero,
                    color: Color.White
                );

                _renderHelper.FinishDraw();
            }

            return _renderHelper.Result;
        }

        public void Update(Vector2 position, int width, int height)
        {
            Child.Update(position, Child.RequiredWidth, Child.RequiredHeight);
        }
    }
}
