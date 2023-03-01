using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameDrawingApp.Ui.Base;
using MonoGameDrawingApp.Ui.Base.Popup;
using MonoGameDrawingApp.Ui.Base.TextInput.Filters;
using MonoGameDrawingApp.Ui.Base.TextInput.Filters.Alphanumeric;
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
            new Color(200, 200, 200),
            new Color(75, 75, 75),
            new Color(200, 0, 0),
            new Color(0, 200, 0),
            new Color(0, 0, 200),
            new Color(0, 200, 200),
            new Color(200, 0, 200),
            new Color(200, 200, 0),
        };

        private readonly RenderHelper _renderHelper;
        private readonly PreviewSpriteAttachment _preview;
        private readonly Camera _camera;

        private readonly Grid _grid;
        private readonly VectorSpriteItemEditor _editor;
        private Vector2? _worldDragStart = null;
        private Vector2? _pixelDragStart = null;
        private int _colorIndex = 0;
        private bool _showGrid = true;
        private float _targetZoom = 50;

        public VectorSpriteViewportView(UiEnvironment environment, VectorSpriteTabView vectorSpriteTabView)
        {
            Environment = environment;
            VectorSpriteTabView = vectorSpriteTabView;

            _preview = new PreviewSpriteAttachment(VectorSpriteTabView.Sprite);
            _renderHelper = new RenderHelper();

            _camera = new Camera()
            {
                Zoom = 50,
                Position = new Vector2(0, 0)
            };
            _grid = new Grid(_camera, 8, Environment.Content);
            _editor = new VectorSpriteItemEditor(Environment.Content, _camera, _grid);

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

                _grid.RenderBackground(graphics.SpriteBatch, width, height, BackgroundColor);

                graphics.SpriteBatch.End();

                graphics.TriangleBatch.Begin();

                _preview.Draw(graphics.TriangleBatch);

                graphics.TriangleBatch.End();

                graphics.SpriteBatch.Begin();

                _editor.Draw(graphics.SpriteBatch, Environment.Font, Util.InvertColor(BackgroundColor));

                if (_showGrid)
                {
                    _grid.RenderGrid(graphics.SpriteBatch, width, height, Util.InvertColor(BackgroundColor) * 0.5f);
                }

                graphics.SpriteBatch.End();

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
                if (mouse.RightButton == ButtonState.Pressed && Environment.OldMouse.RightButton == ButtonState.Released)
                {
                    _startDrag();
                }
                _targetZoom = Math.Clamp(_targetZoom + (mouse.ScrollWheelValue - Environment.OldMouse.ScrollWheelValue) / 1000f * _camera.Zoom, 5, 5000);
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

            _editor.Selected = treeItem?.Item;

            _editor.Update(position);

            if (_editor.Changed)
            {
                Changed = true;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.LeftShift) != Environment.OldKeyboardState.IsKeyDown(Keys.LeftShift))
            {
                Changed = true;
            }

            if (Environment.JustPressed(Keys.F) && _editor.Selected is VectorSpriteItem selected)
            {
                _camera.Position = selected.AbsolutePosition;
            }

            if (Environment.JustPressed(Keys.G))
            {
                if (Keyboard.GetState().IsKeyDown(Keys.LeftControl))
                {
                    _updateGridResolution();
                }
                else
                {
                    _showGrid = !_showGrid;
                    Changed = true;
                }

            }

            _updateZoom(dragging);
        }

        private void _updateGridResolution()
        {
            VectorSpriteTabView.PopupEnvironment.OpenCentered(new TextInputPopup(
                environment: Environment,
                popupEnvironment: VectorSpriteTabView.PopupEnvironment,
                confirmed: (string newValue) =>
                {
                    try
                    {
                        _grid.Resolution = Math.Clamp(int.Parse(newValue), 0, 512);
                        _showGrid = true;
                        Changed = true;
                    }
                    catch { }
                },
                filters: new ITextInputFilter[] { new NumericTextInputFilter() },
                title: "Set Grid Resolution (0 for no grid)"
            ));
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