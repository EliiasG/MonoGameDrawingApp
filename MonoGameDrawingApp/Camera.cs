using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameDrawingApp
{
    public class Camera
    {
        private float _left;
        private float _right;
        private float _bottom;
        private float _top;

        private Vector2 _position;
        private float _zoom;

        public Camera(float zoom = 1, Vector2 position = new Vector2())
        {
            Zoom = zoom;
            Position = position;
            Changed = true;
        }

        public float Zoom
        {
            get => _zoom;
            set
            {
                if (_zoom != value)
                {
                    _zoom = value;
                    Changed = true;
                }
            }
        }

        public bool Changed { get; set; }

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

        public void Apply(BasicEffect effect, int width, int height)
        {
            Update(width, height);

            effect.Projection = Matrix.CreateOrthographicOffCenter(_left, _right, _bottom, _top, 0f, 1f);

            Changed = false;
        }

        public void Update(int width, int height)
        {
            _left = -width / 2f / Zoom + Position.X;
            _right = width / 2f / Zoom + Position.X;
            _bottom = -height / 2f / Zoom + Position.Y;
            _top = height / 2f / Zoom + Position.Y;
        }

        public Vector2 PixelToWorld(Vector2 pixel)
        {
            float x = _left + pixel.X / Zoom;
            float y = _top - pixel.Y / Zoom;
            return new Vector2(x, y);
        }

        public Vector2 WorldToPixel(Vector2 world)
        {
            float x = (world.X - _left) * Zoom;
            float y = (_top - world.Y) * Zoom;
            return new Vector2(x, y);
        }
    }
}
