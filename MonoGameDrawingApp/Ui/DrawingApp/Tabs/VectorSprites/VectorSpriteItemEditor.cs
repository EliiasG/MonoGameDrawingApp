using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameDrawingApp.VectorSprites;
using System.Globalization;

namespace MonoGameDrawingApp.Ui.DrawingApp.Tabs.VectorSprites
{
    public class VectorSpriteItemEditor
    {
        private const int SelectDistance = 25;
        private const int SelectDistanceSquared = SelectDistance * SelectDistance;

        private const int AddOnLineDistance = 10;
        private const int AddOnLineDistanceSquared = AddOnLineDistance * AddOnLineDistance;

        private static Texture2D _originDefault;
        private static Texture2D _originSelected;
        private static Texture2D _vertexDefault;
        private static Texture2D _vertexSelected;

        private VectorSpriteItem _selected;
        private bool _isInOriginMode;
        private bool _isNearOrigin;
        private Vector2? _dragOffset;
        private MouseState _oldMouse;
        private Vector2? _dragPosition;
        private bool _isInAddMode;
        private int _selectedIndex = -1;

        public VectorSpriteItemEditor(ContentManager contentManager, Camera camera, Grid grid)
        {
            LoadImages(contentManager);
            Camera = camera;
            Grid = grid;
        }

        public Camera Camera { get; init; }

        public Grid Grid { get; init; }

        public VectorSpriteItem Selected
        {
            get => _selected;
            set
            {
                if (value != _selected)
                {
                    _selected = value;
                    Changed = true;
                }
            }
        }

        public bool Changed { get; private set; }

        public void Draw(SpriteBatch spriteBatch, SpriteFont font, Color color)
        {
            if (Selected == null)
            {
                return;
            }
            if (_isInOriginMode || _isInAddMode)
            {
                RenderIndices(spriteBatch, font, color);
                RenderOrigin(spriteBatch, _isNearOrigin || _dragOffset != null ? _originSelected : _originDefault, color);
            }
            else
            {
                RenderVertices(spriteBatch, color);
                RenderOrigin(spriteBatch, _originDefault, color);
            }
            if (_dragPosition != null && _selectedIndex == -1 && !_isInOriginMode)
            {
                spriteBatch.Draw(
                    texture: _vertexSelected,
                    position: _dragPosition.Value - (_vertexSelected.Bounds.Size.ToVector2() / 2),
                    color: color
                );
            }
            Changed = false;
        }

        public void Update(Vector2 position)
        {
            MouseState mouse = Mouse.GetState();
            KeyboardState keyboard = Keyboard.GetState();

            bool addMode = keyboard.IsKeyDown(Keys.LeftControl);

            bool originMode = keyboard.IsKeyDown(Keys.LeftShift) && !addMode;

            if (originMode != _isInOriginMode)
            {
                _isInOriginMode = originMode;
                _dragOffset = null;
                Changed = true;
            }

            if (addMode != _isInAddMode)
            {
                _isInAddMode = addMode;
                _dragOffset = null;
                Changed = true;
            }


            if (Selected != null)
            {
                if (_isInAddMode)
                {
                    UpdateAddMode(position);
                }
                else if (_isInOriginMode)
                {
                    UpdateOriginMode(position);
                }
                else
                {
                    UpdateVertexMode(position);
                }
            }

            _oldMouse = mouse;
        }

        private static void LoadImages(ContentManager contentManager)
        {
            _originDefault ??= contentManager.Load<Texture2D>("icons/originDefault");
            _originSelected ??= contentManager.Load<Texture2D>("icons/originSelected");
            _vertexDefault ??= contentManager.Load<Texture2D>("icons/vertexDefault");
            _vertexSelected ??= contentManager.Load<Texture2D>("icons/vertexSelected");
        }

        private void UpdateVertexMode(Vector2 position)
        {
            MouseState mouse = Mouse.GetState();

            bool clicking = mouse.LeftButton == ButtonState.Pressed;
            bool justClikced = clicking && _oldMouse.LeftButton == ButtonState.Released;

            int closestVertexIndex = 0;
            float closestVertexDistSq = SelectDistanceSquared + 1;

            Vector2 mousePos = mouse.Position.ToVector2() - position;

            int i = 0;
            if (_dragOffset == null)
            {

                foreach (Vector2 vertex in Selected.Geometry.Points)
                {
                    Vector2 vertexPixelPosition = Camera.WorldToPixel(vertex + Selected.AbsolutePosition);
                    float distSq = Util.DistanceSquared(vertexPixelPosition, mousePos);
                    if (distSq < closestVertexDistSq)
                    {
                        closestVertexIndex = i;
                        closestVertexDistSq = distSq;
                    }
                    ++i;
                }

                if (closestVertexDistSq < SelectDistanceSquared)
                {
                    Changed = true;
                    _selectedIndex = closestVertexIndex;
                }
                else if (_selectedIndex != -1)
                {
                    Changed = true;
                    _selectedIndex = -1;
                }
                else
                {
                    UpdatePlaceOnLine(mousePos);
                }

                if (_selectedIndex != -1 && justClikced)
                {
                    if (Keyboard.GetState().IsKeyDown(Keys.LeftAlt))
                    {
                        Selected.Geometry.RemovePointAt(_selectedIndex);
                        return;
                    }
                    Vector2 vertexPos = Selected.Geometry.PointAt(_selectedIndex) + Selected.AbsolutePosition;
                    vertexPos = Camera.WorldToPixel(vertexPos);

                    _dragOffset = mousePos - vertexPos;
                }
            }
            else
            {
                Vector2 newDragPos = Camera.PixelToWorld(mousePos - _dragOffset.Value);
                newDragPos = Grid.Snap(newDragPos);
                if (mouse.LeftButton == ButtonState.Released)
                {
                    Selected.Geometry.ReplacePointAt(_selectedIndex, newDragPos.ToNumerics() - Selected.AbsolutePosition);
                    _selectedIndex = -1;
                    _dragPosition = null;
                    _dragOffset = null;
                    return;
                }
                Vector2 ScreenDragPos = Camera.WorldToPixel(newDragPos);
                if (ScreenDragPos != _dragPosition)
                {
                    _dragPosition = ScreenDragPos;
                    Changed = true;
                }
            }
        }

        private void UpdatePlaceOnLine(Vector2 mousePos)
        {
            float closestLineDistSq = AddOnLineDistanceSquared + 1;
            int size = Selected.Geometry.Size;
            Vector2 closestPoint = Vector2.Zero;
            int closestIndex = -1;

            for (int i = 0; i < size; i++)
            {
                Vector2 current = Selected.Geometry.PointAt(i) + Selected.AbsolutePosition;
                Vector2 next = Selected.Geometry.PointAt((i + 1) % size) + Selected.AbsolutePosition;

                current = Camera.WorldToPixel(current);
                next = Camera.WorldToPixel(next);

                Vector2 onLine = ExtraMath.ClosestPointOnLine(mousePos.ToNumerics(), current.ToNumerics(), next.ToNumerics());

                float distSq = (onLine - mousePos).LengthSquared();
                if (distSq < closestLineDistSq)
                {
                    closestLineDistSq = distSq;
                    closestPoint = onLine;
                    closestIndex = i;
                }
            }

            if (closestLineDistSq <= AddOnLineDistanceSquared)
            {
                if (_dragPosition != closestPoint)
                {
                    _dragPosition = closestPoint;
                    Changed = true;
                }
                if (Mouse.GetState().LeftButton == ButtonState.Pressed && _oldMouse.LeftButton == ButtonState.Released)
                {
                    _selectedIndex = closestIndex + 1;
                    Selected.Geometry.InsertPointAt(closestIndex + 1, Camera.PixelToWorld(closestPoint).ToNumerics() - Selected.AbsolutePosition);
                    _dragOffset = Vector2.Zero;
                }
            }
            else if (_dragPosition != null)
            {
                _dragPosition = null;
                Changed = true;
            }

        }

        private void UpdateOriginMode(Vector2 position)
        {
            MouseState mouse = Mouse.GetState();

            _selectedIndex = -1;

            bool clicking = mouse.LeftButton == ButtonState.Pressed;

            bool justClikced = clicking && _oldMouse.LeftButton == ButtonState.Released;

            Vector2 mousePosition = mouse.Position.ToVector2() - position;
            Vector2 originPixelPos = Camera.WorldToPixel(Selected.AbsolutePosition);
            float distSquared = Util.DistanceSquared(mousePosition, originPixelPos);

            bool nearOrigin = distSquared <= SelectDistanceSquared;

            if (nearOrigin != _isNearOrigin)
            {
                Changed = true;
                _isNearOrigin = nearOrigin;
            }

            if (nearOrigin && justClikced)
            {
                _dragOffset = mousePosition - originPixelPos;
            }

            if (_dragOffset is Vector2 dragOffset)
            {
                Vector2 newPos = mousePosition + dragOffset;
                newPos = Camera.PixelToWorld(newPos);
                newPos = Grid.Snap(newPos);
                _dragPosition = newPos;

                if (Selected.Position != newPos.ToNumerics())
                {
                    Changed = true;
                }

                if (!clicking)
                {
                    Selected.AbsolutePosition = newPos.ToNumerics();
                    _dragOffset = null;
                }
            }
            else
            {
                _dragPosition = null;
            }
        }

        private void UpdateAddMode(Vector2 position)
        {
            MouseState mouse = Mouse.GetState();

            bool clicking = mouse.LeftButton == ButtonState.Pressed;
            bool justClikced = clicking && _oldMouse.LeftButton == ButtonState.Released;

            _selectedIndex = -1;

            if (_isNearOrigin || _dragPosition != null)
            {
                Changed = true;
                _isNearOrigin = false;
                _dragPosition = null;
            }

            if (justClikced)
            {
                Vector2 pos = Camera.PixelToWorld(mouse.Position.ToVector2() - position);
                Selected.Geometry.AddPoint(Grid.Snap(pos - Selected.AbsolutePosition).ToNumerics());
            }
        }

        private void RenderOrigin(SpriteBatch spriteBatch, Texture2D texture, Color color)
        {
            Vector2 position = Camera.WorldToPixel(_isInOriginMode ? _dragPosition ?? Selected.AbsolutePosition : Selected.AbsolutePosition);//Selected.AbsolutePosition);
            spriteBatch.Draw(
                texture: texture,
                position: position - (_originDefault.Bounds.Size.ToVector2() / 2),
                color: color
            );
        }

        private void RenderIndices(SpriteBatch spriteBatch, SpriteFont font, Color color)
        {
            int i = 0;
            foreach (Vector2 point in Selected.Geometry.Points)
            {
                string text = i.ToString(NumberFormatInfo.InvariantInfo);

                spriteBatch.DrawString(
                    spriteFont: font,
                    text: text,
                    position: Camera.WorldToPixel(point + Selected.AbsolutePosition) - (font.MeasureString(text) / 2),
                    color: color//Util.InvertColor(BackgroundColor)
                );

                ++i;
            }
        }

        private void RenderVertices(SpriteBatch spriteBatch, Color color)
        {
            int i = 0;

            foreach (Vector2 point in Selected.Geometry.Points)
            {
                Texture2D texture = _selectedIndex == i ? _vertexSelected : _vertexDefault;
                Vector2 position =
                    _selectedIndex == i && _dragPosition != null && _dragOffset != null ?
                    _dragPosition.Value :
                    Camera.WorldToPixel(point + Selected.AbsolutePosition);

                spriteBatch.Draw(
                    texture: texture,
                    position: position - (texture.Bounds.Size.ToVector2() / 2),
                    color: color
                );
                ++i;
            }
        }
    }
}
