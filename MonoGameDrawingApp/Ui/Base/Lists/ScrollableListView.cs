using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameDrawingApp.Ui.Base.Scroll;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoGameDrawingApp.Ui.Base.Lists
{
    public abstract class ScrollableListView : IScrollableView
    {
        public IEnumerable<IUiElement> Items;

        public readonly bool UpdateOutOfView;
        public readonly int Spacing;

        private readonly UiEnvironment _environment;
        private readonly RenderHelper _renderHelper;

        private IEnumerable<(Rectangle, IUiElement)> _placedItems;
        private IList<IUiElement> _oldItems;
        private Vector2 _position;
        private int _height = 1;
        private int _width = 1;
        private int _maxHeight = 1;
        private int _maxWidth = 1;
        private bool _changed;

        protected ScrollableListView(UiEnvironment environment, IEnumerable<IUiElement> items, bool updateOutOfView, int spacing)
        {
            _environment = environment;
            Items = items;
            UpdateOutOfView = updateOutOfView;
            Spacing = spacing;
            _renderHelper = new RenderHelper();
            _placedItems = new (Rectangle, IUiElement)[0];
        }

        public Vector2 Position
        {
            get
            {
                return _position;
            }
            set
            {
                if (_position != value)
                {
                    _position = value;
                    _changed = true;
                }
            }
        }

        public int Width => _width;

        public int Height => _height;

        public int MaxWidth => _maxWidth; // + 1 to not have the scrollbar divide by 0

        public int MaxHeight => _maxHeight;

        public bool Changed => _changed;

        public int RequiredWidth => 1;

        public int RequiredHeight => 1;

        public UiEnvironment Environment => _environment;

        public Texture2D Render(Graphics graphics, int width, int height)
        {
            _renderHelper.Begin(graphics, width, height);

            Rectangle _view = new(0, 0, width, height);

            if (_changed || _renderHelper.SizeChanged)
            {
                IList<(Vector2, Texture2D)> renders = new List<(Vector2, Texture2D)>();

                foreach ((Rectangle, IUiElement) item in _placedItems)
                {
                    Rectangle itemRect = item.Item1;
                    Rectangle movedRect = new(itemRect.Location - _position.ToPoint(), itemRect.Size);

                    if (_view.Intersects(movedRect))
                    {
                        Texture2D render = item.Item2.Render(graphics, itemRect.Width, itemRect.Height);
                        renders.Add((movedRect.Location.ToVector2(), render));
                    }
                }

                _renderHelper.BeginDraw();

                foreach ((Vector2, Texture2D) render in renders)
                {
                    graphics.SpriteBatch.Draw(
                        texture: render.Item2,
                        position: render.Item1,
                        color: Color.White
                    );
                }

                _renderHelper.FinishDraw();
            }
            _changed = false;
            return _renderHelper.Result;
        }

        public void Update(Vector2 position, int width, int height)
        {
            _height = height;
            _width = width;

            if (_childrenChanged())
            {
                _placedItems = _positionItems();
            }

            Rectangle _view = new(0, 0, width, height);
            _maxWidth = 1;
            _maxHeight = 1;

            foreach ((Rectangle, IUiElement) item in _placedItems)
            {
                Rectangle itemRect = item.Item1;
                Point pos = itemRect.Location;
                IUiElement element = item.Item2;

                Point outer = pos + itemRect.Size;
                _maxHeight = Math.Max(outer.Y, _maxHeight);
                _maxWidth = Math.Max(outer.X, _maxWidth);

                Rectangle movedRect = new(itemRect.Location - _position.ToPoint(), itemRect.Size);
                bool intersects = _view.Intersects(movedRect);

                if (intersects)
                {
                    _changed = _changed || element.Changed;
                }

                if (UpdateOutOfView || intersects)
                {
                    element.Update(position + movedRect.Location.ToVector2(), itemRect.Width, itemRect.Height);
                }
            }
        }

        private bool _childrenChanged()
        {

            if (_oldItems?.Count() != Items.Count())
            {
                goto FoundChange;
            }

            int i = 0;
            // foreach with index variable, becauase you cannot index in IEnumerable
            foreach (IUiElement item in Items)
            {
                if (item != _oldItems[i] || item.Changed)
                {
                    goto FoundChange;
                }
                ++i;
            }

            return false;

        FoundChange:
            _oldItems = new List<IUiElement>(Items);
            _changed = true;
            return true;
        }

        protected abstract IEnumerable<(Rectangle, IUiElement)> _positionItems();
    }
}
