using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameDrawingApp.Ui.Base;

namespace MonoGameDrawingApp.Ui
{
    public class ScaleView : IUiElement
    {
        public readonly IUiElement Child;
        public readonly bool DisableBlur;

        private readonly UiEnvironment _environment;

        private readonly RenderHelper _renderHelper;

        public ScaleView(UiEnvironment environment, IUiElement child, bool disableBlur = false)
        {
            _environment = environment;
            DisableBlur = disableBlur;
            Child = child;
            _renderHelper = new RenderHelper();
        }

        public int RequiredWidth => 1;

        public int RequiredHeight => 1;

        public bool Changed => Child.Changed;

        public UiEnvironment Environment => _environment;

        public Texture2D Render(Graphics graphics, int width, int height)
        {
            _renderHelper.Begin(graphics, width, height);

            if (Changed || _renderHelper.SizeChanged)
            {
                Texture2D childRender = Child.Render(graphics, Child.RequiredWidth, Child.RequiredHeight);

                _renderHelper.BeginDraw();
                // to disable blur
                if (DisableBlur)
                {
                    graphics.SpriteBatch.End();
                    graphics.SpriteBatch.Begin(samplerState: SamplerState.PointClamp);
                }

                graphics.SpriteBatch.Draw(
                    texture: childRender,
                    destinationRectangle: new Rectangle(0, 0, width, height),
                    color: Color.White
                );

                _renderHelper.FinishDraw();
            }

            return _renderHelper.Result;
        }

        public void Update(Vector2 position, int width, int height)
        {
            Child.Update(position, width, height);
        }
    }
}
