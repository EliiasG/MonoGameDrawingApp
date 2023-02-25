using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameDrawingApp.Ui.Base;
using MonoGameDrawingApp.Ui.DrawingApp.Tabs.VectorSprites.Rendering;
using MonoGameDrawingApp.Ui.DrawingApp.Tabs.VectorSprites.Tree;
using MonoGameDrawingApp.VectorSprites;
using System;

namespace MonoGameDrawingApp.Ui.DrawingApp.Tabs.VectorSprites.Elements
{
    public class VectorSpriteViewportView : IUiElement
    {
        private const float ZoomSpeed = 0.1f;

        private readonly static Color[] s_backgroundColors = new Color[]
        {
            new Color(255, 255, 255),
            new Color(255, 0, 0),
            new Color(0, 255, 0),
            new Color(0, 0, 255),
            new Color(0, 255, 255),
            new Color(255, 0, 255),
            new Color(255, 255, 0),
        };

        private readonly RenderHelper _renderHelper;
        private readonly PreviewSpriteAttachment _preview;
        private readonly Camera _camera;

        private Vector2? _worldDragStart = null;
        private Vector2? _pixelDragStart = null;
        private int _colorIndex = 0;
        private MouseState _oldMouse;
        private VectorSpriteItem _selected;
        private float _targetZoom = 50;
        private static Texture2D _grid;

        public VectorSpriteViewportView(UiEnvironment environment, VectorSpriteTabView vectorSpriteTabView)
        {
            Environment = environment;
            VectorSpriteTabView = vectorSpriteTabView;

            _preview = new PreviewSpriteAttachment(VectorSpriteTabView.Sprite);
            _renderHelper = new RenderHelper();
            _grid ??= environment.Content.Load<Texture2D>("grid");
            _camera = new Camera()
            {
                Zoom = 50,
                Position = new Vector2(0, 0)
            };

            VectorSpriteTabView.Sprite.AddAttachment(_preview);

            vectorSpriteTabView.ChangeListener.Changed += () =>
            {
                Changed = true;
            };

        }

        public VectorSpriteTabView VectorSpriteTabView { get; init; }

        public Color BackgroundColor => s_backgroundColors[_colorIndex];

        public bool Changed { get; private set; }

        public int RequiredWidth => 1;

        public int RequiredHeight => 1;

        public UiEnvironment Environment { get; init; }

        public Texture2D Render(Graphics graphics, int width, int height)
        {
            _renderHelper.Begin(graphics, width, height);

            KeyboardState keyboard = Keyboard.GetState();

            if (_renderHelper.SizeChanged || Changed)
            {
                _renderHelper.BeginDraw();
                _camera.Apply(graphics.TriangleBatch.Effect, width, height);
                graphics.SpriteBatch.Begin(samplerState: SamplerState.PointClamp);

                _renderGrid(graphics.SpriteBatch, width, height);

                graphics.SpriteBatch.End();

                graphics.TriangleBatch.Begin();

                _preview.Draw(graphics.TriangleBatch);

                graphics.TriangleBatch.End();
                if (_selected is VectorSpriteItem item)
                {
                    graphics.SpriteBatch.Begin();

                    _renderIndices(graphics.SpriteBatch, item);

                    graphics.SpriteBatch.End();
                }

                _renderHelper.FinishDraw();
            }

            Changed = false;

            bool ctrlShift = keyboard.IsKeyDown(Keys.LeftControl) && keyboard.IsKeyDown(Keys.LeftShift);

            if (ctrlShift && Environment.JustPressed(Keys.W))
            {
                graphics.TriangleBatch.IsWireframe = !graphics.TriangleBatch.IsWireframe;
                Changed = true;
            }

            if (ctrlShift && Environment.JustPressed(Keys.C))
            {
                _colorIndex = ++_colorIndex % s_backgroundColors.Length;
                Changed = true;
            }

            return _renderHelper.Result;
        }

        private void _renderIndices(SpriteBatch spriteBatch, VectorSpriteItem item)
        {


            int i = 0;
            foreach (Vector2 point in item.Geometry.Points)
            {
                spriteBatch.DrawString(Environment.Font, i.ToString(), _camera.WorldToPixel(point + item.AbsolutePosition), Color.White);
                ++i;
            }
        }

        private void _renderGrid(SpriteBatch spriteBatch, int width, int height)
        {
            float zoom = _camera.Zoom * 2;
            int horizontalAmount = (int)(width / zoom + 4);
            int verticalAmount = (int)(height / zoom + 4);

            Vector2 start = new(-_camera.Position.X * _camera.Zoom % zoom, _camera.Position.Y * _camera.Zoom % zoom);
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
                        color: BackgroundColor,
                        rotation: 0f,
                        origin: Vector2.Zero,
                        scale: zoom / 2,
                        effects: SpriteEffects.None,
                        layerDepth: 0f
                    );
                }
            }
        }

        public void Update(Vector2 position, int width, int height)
        {
            MouseState mouse = Mouse.GetState();

            _camera.Update(width, height);
            if (_camera.Changed)
            {
                Changed = true;
            }

            Rectangle bounds = new Rectangle(position.ToPoint(), new Point(width, height));
            if (bounds.Contains(mouse.Position))
            {
                if (mouse.RightButton == ButtonState.Pressed && _oldMouse.RightButton == ButtonState.Released)
                {
                    _startDrag();
                }
                _targetZoom = Math.Clamp(_targetZoom + (mouse.ScrollWheelValue - _oldMouse.ScrollWheelValue) / 1000f * _camera.Zoom, 5, 5000);
            }

            bool dragging = _worldDragStart != null && mouse.RightButton == ButtonState.Pressed;

            if (dragging)
            {
                Vector2 dragAmount = (Vector2)_pixelDragStart - mouse.Position.ToVector2();
                dragAmount.Y *= -1;
                _camera.Position = (Vector2)_worldDragStart + dragAmount / _camera.Zoom;
            }
            else
            {
                _worldDragStart = null;
                _pixelDragStart = null;
            }

            VectorSpriteTreeItem treeItem = VectorSpriteTabView.Tree.Selected as VectorSpriteTreeItem;

            if (treeItem?.Item != _selected)
            {
                _selected = treeItem?.Item;
                Changed = true;
            }

            if (Environment.JustPressed(Keys.F) && _selected is VectorSpriteItem selected)
            {
                _camera.Position = selected.AbsolutePosition;
            }

            _updateZoom(dragging);

            _oldMouse = mouse;
        }

        private void _updateZoom(bool dragging)
        {
            float zoomDiff = _camera.Zoom - _targetZoom;

            if (dragging && zoomDiff != 0)
            {
                _startDrag();
            }

            float speed = ZoomSpeed * Math.Abs(zoomDiff);

            _camera.Zoom -= Math.Clamp(zoomDiff, -speed - ZoomSpeed, speed + ZoomSpeed);
        }

        private void _startDrag()
        {
            _worldDragStart = _camera.Position;
            _pixelDragStart = Mouse.GetState().Position.ToVector2();
        }
    }
}
