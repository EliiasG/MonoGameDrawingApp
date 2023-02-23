using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameDrawingApp.Ui.Base;
using MonoGameDrawingApp.Ui.DrawingApp.Tabs.VectorSprites.Rendering;

namespace MonoGameDrawingApp.Ui.DrawingApp.Tabs.VectorSprites.Elements
{
    public class VectorSpriteViewportView : IUiElement
    {
        //TODO change
        private const int Zoom = 50;

        private readonly RenderHelper _renderHelper;

        private readonly PreviewSpriteAttachment _preview;

        public VectorSpriteViewportView(UiEnvironment environment, VectorSpriteTabView vectorSpriteTabView)
        {
            Environment = environment;
            VectorSpriteTabView = vectorSpriteTabView;

            _preview = new PreviewSpriteAttachment(VectorSpriteTabView.Sprite);
            _renderHelper = new RenderHelper();

            VectorSpriteTabView.Sprite.AddAttachment(_preview);
        }

        public VectorSpriteTabView VectorSpriteTabView { get; init; }

        public bool Changed => _renderHelper.SizeChanged; //TODO

        public int RequiredWidth => 1;

        public int RequiredHeight => 1;

        public UiEnvironment Environment { get; init; }

        public Texture2D Render(Graphics graphics, int width, int height)
        {
            _renderHelper.Begin(graphics, width, height);

            KeyboardState keyboard = Keyboard.GetState();

            if (keyboard.IsKeyDown(Keys.LeftControl) && keyboard.IsKeyDown(Keys.LeftShift) && keyboard.IsKeyDown(Keys.W) && Environment.OldKeyboardState.IsKeyUp(Keys.W))
            {
                graphics.TriangleBatch.IsWireframe = !graphics.TriangleBatch.IsWireframe;
            }

            if (_renderHelper.SizeChanged)
            {
                _renderHelper.BeginDraw();
                graphics.TriangleBatch.Effect.Projection = Matrix.CreateOrthographicOffCenter(-width / 2 / Zoom, width / 2 / Zoom, -height / 2 / Zoom, height / 2 / Zoom, 0f, 1f);
                graphics.TriangleBatch.Begin();

                _preview.Draw(graphics.TriangleBatch);

                graphics.TriangleBatch.End();
                _renderHelper.FinishDraw();
            }

            return _renderHelper.Result;
        }

        public void Update(Vector2 position, int width, int height)
        {

        }
    }
}
