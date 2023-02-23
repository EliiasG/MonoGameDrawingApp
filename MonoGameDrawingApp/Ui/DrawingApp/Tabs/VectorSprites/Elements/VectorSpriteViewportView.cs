using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameDrawingApp.Ui.Base;

namespace MonoGameDrawingApp.Ui.DrawingApp.Tabs.VectorSprites.Elements
{
    public class VectorSpriteViewportView : IUiElement
    {
        //TODO change
        private const int Zoom = 50;

        private RenderHelper _renderHelper;

        public VectorSpriteViewportView(UiEnvironment environment, VectorSpriteTabView vectorSpriteTabView)
        {
            Environment = environment;

            _renderHelper = new RenderHelper();
        }

        public bool Changed => _renderHelper.SizeChanged; //TODO

        public int RequiredWidth => 1;

        public int RequiredHeight => 1;

        public UiEnvironment Environment { get; init; }

        public Texture2D Render(Graphics graphics, int width, int height)
        {
            _renderHelper.Begin(graphics, width, height);

            if (_renderHelper.SizeChanged)
            {
                _renderHelper.BeginDraw();
                graphics.TriangleBatch.Effect.Projection = Matrix.CreateOrthographicOffCenter(-width / 2 / Zoom, width / 2 / Zoom, -height / 2 / Zoom, height / 2 / Zoom, 0f, 1f);
                graphics.TriangleBatch.Begin();

                VertexPositionColor[] vertexPositionColors = new VertexPositionColor[]
                {
                    new VertexPositionColor(new Vector3(0, 0, 0), new Color(255, 0, 0)),
                    new VertexPositionColor(new Vector3(0, 2, 0), new Color(0, 255, 0)),
                    new VertexPositionColor(new Vector3(2, 0, 0), new Color(0, 0, 255)),
                };

                int[] indices = new int[] { 0, 1, 2 };

                graphics.TriangleBatch.DrawTriangles(vertexPositionColors, indices);

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
