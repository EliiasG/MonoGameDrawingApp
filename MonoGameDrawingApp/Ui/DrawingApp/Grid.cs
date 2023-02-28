using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameDrawingApp.Ui.DrawingApp
{
    public class Grid
    {
        private static Texture2D _pixel;
        private static Texture2D _grid;

        public Grid(Camera camera, int resolution, ContentManager contentManager)
        {
            Camera = camera;
            Resolution = resolution;

            _pixel ??= contentManager.Load<Texture2D>("pixel");
            _grid ??= contentManager.Load<Texture2D>("grid");
        }

        public Camera Camera { get; init; }

        public int Resolution { get; set; }

        public void RenderGrid(SpriteBatch spriteBatch, int width, int height, Color color)
        {
            if (Resolution == 0)
            {
                return;
            }

            float spacing = Camera.Zoom / Resolution;

            int horizontalAmount = (int)(width / spacing) + 3;
            int verticalAmount = (int)(height / spacing) + 3;

            Vector2 offset = Camera.Position * Camera.Zoom;

            for (int i = 0; i < horizontalAmount; i++)
            {
                float screenOffset = width / 2 % spacing;
                float positionOffset = offset.X % spacing;
                int position = (int)(i * spacing + screenOffset - positionOffset - spacing);
                spriteBatch.Draw(
                    texture: _pixel,
                    destinationRectangle: new Rectangle(position - 1, 0, 2, height),
                    color: color
                );
            }

            for (int i = 0; i < verticalAmount; i++)
            {
                float screenOffset = height / 2 % spacing;
                float positionOffset = -offset.Y % spacing;
                int position = (int)(i * spacing + screenOffset - positionOffset - spacing);
                spriteBatch.Draw(
                    texture: _pixel,
                    destinationRectangle: new Rectangle(0, position - 1, width, 2),
                    color: color
                );
            }
        }

        public void RenderBackground(SpriteBatch spriteBatch, int width, int height, Color color)
        {
            float zoom = Camera.Zoom * 2;
            int horizontalAmount = (int)(width / zoom + 4);
            int verticalAmount = (int)(height / zoom + 4);

            Vector2 start = new(-Camera.Position.X * Camera.Zoom % zoom, Camera.Position.Y * Camera.Zoom % zoom);
            start -= new Vector2(zoom) * 2;
            start += new Vector2(width / 2 % zoom, height / 2 % zoom);

            for (int x = 0; x < horizontalAmount; x++)
            {
                for (int y = 0; y < verticalAmount; y++)
                {
                    spriteBatch.Draw(
                        texture: _grid,
                        position: start + new Vector2(x * zoom, y * zoom),
                        sourceRectangle: null,
                        color: color,
                        rotation: 0f,
                        origin: Vector2.Zero,
                        scale: zoom / 2,
                        effects: SpriteEffects.None,
                        layerDepth: 0f
                    );
                }
            }
        }

        public Vector2 Snap(Vector2 position)
        {
            if (Resolution == 0)
            {
                return position;
            }

            position *= Resolution;
            position -= new Vector2(0.5f);
            position.Ceiling();
            return position / Resolution;
        }
    }
}
