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
        private readonly RenderHelper _renderHelper;

        private IEnumerable<(Rectangle, IUiElement)> _placedItems;
        private IList<IUiElement> _oldItems;
        private Vector2 _position;

        protected ScrollableListView(UiEnvironment environment, IEnumerable<IUiElement> items, bool updateOutOfView, int spacing)
        {
            Environment = environment;
            Items = items;
            UpdateOutOfView = updateOutOfView;
            Spacing = spacing;
            _renderHelper = new RenderHelper();
            _placedItems = Array.Empty<(Rectangle, IUiElement)>();
        }

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

        public int Width { get; private set; } = 1;

        public int Height { get; private set; } = 1;

        public int MaxWidth { get; private set; } = 1; // + 1 to not have the scrollbar divide by 0

        public int MaxHeight { get; private set; } = 1;

        public bool Changed { get; private set; }

        public int RequiredWidth => 1;

        public int RequiredHeight => 1;

        public UiEnvironment Environment { get; }

        public int Spacing { get; }

        public bool UpdateOutOfView { get; }
        public IEnumerable<IUiElement> Items { get; set; }

        public Texture2D Render(Graphics graphics, int width, int height)
        {
            _renderHelper.Begin(graphics, width, height);

            Rectangle _view = new(0, 0, width, height);

            if (Changed || _renderHelper.SizeChanged)
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

                _renderHelper.BeginSpriteBatchDraw();

                foreach ((Vector2, Texture2D) render in renders)
                {
                    graphics.SpriteBatch.Draw(
                        texture: render.Item2,
                        position: render.Item1,
                        color: Color.White
                    );
                }

                _renderHelper.FinishSpriteBatchDraw();
            }
            Changed = false;
            return _renderHelper.Result;
        }

        public void Update(Vector2 position, int width, int height)
        {
            Height = height;
            Width = width;

            if (ChildrenChanged())
            {
                _placedItems = PositionItems();
            }

            Rectangle _view = new(0, 0, width, height);
            MaxWidth = 1;
            MaxHeight = 1;

            foreach ((Rectangle, IUiElement) item in _placedItems)
            {
                Rectangle itemRect = item.Item1;
                Point pos = itemRect.Location;
                IUiElement element = item.Item2;

                Point outer = pos + itemRect.Size;
                MaxHeight = Math.Max(outer.Y, MaxHeight);
                MaxWidth = Math.Max(outer.X, MaxWidth);

                Rectangle movedRect = new(itemRect.Location - _position.ToPoint(), itemRect.Size);
                bool intersects = _view.Intersects(movedRect);

                if (intersects)
                {
                    Changed = Changed || element.Changed;
                }

                if (UpdateOutOfView || intersects)
                {
                    element.Update(position + movedRect.Location.ToVector2(), itemRect.Width, itemRect.Height);
                }
            }
        }

        private bool ChildrenChanged()
        {

            if (_oldItems?.Count != Items.Count())
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
            Changed = true;
            return true;
        }

        protected abstract IEnumerable<(Rectangle, IUiElement)> PositionItems();
    }
}
